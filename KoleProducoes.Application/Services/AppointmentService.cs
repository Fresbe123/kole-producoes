using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;

namespace KoleProducoes.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<AvailableSlot> _slotRepository;

    public AppointmentService(
        IRepository<Appointment> appointmentRepository,
        IRepository<AvailableSlot> slotRepository)
    {
        _appointmentRepository = appointmentRepository;
        _slotRepository = slotRepository;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
        => await _appointmentRepository.GetAllAsync();

    public async Task<Appointment?> GetByIdAsync(int id)
        => await _appointmentRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status)
        => await _appointmentRepository.FindAsync(a => a.Status == status);

    public async Task<int> GetPendingCountAsync()
    {
        var pending = await _appointmentRepository.FindAsync(
            a => a.Status == AppointmentStatus.Pending);
        return pending.Count();
    }

    public async Task UpdateStatusAsync(int id, AppointmentStatus status, string? internalNotes)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null) return;

        appointment.Status = status;

        if (!string.IsNullOrEmpty(internalNotes))
            appointment.InternalNotes = internalNotes;

        if (status == AppointmentStatus.Cancelled)
        {
            var slot = await _slotRepository.GetByIdAsync(appointment.AvailableSlotId);
            if (slot != null)
            {
                slot.IsBooked = false;
                await _slotRepository.UpdateAsync(slot);
            }
        }

        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task<IEnumerable<AvailableSlot>> GetAllSlotsAsync()
        => await _slotRepository.GetAllAsync();

    public async Task<IEnumerable<AvailableSlot>> GetAvailableSlotsAsync()
        => await _slotRepository.FindAsync(s => s.IsActive && !s.IsBooked);

    public async Task<AvailableSlot> CreateSlotAsync(DateTime dateTime)
    {
        var existing = await _slotRepository.FindAsync(s => s.DateTime == dateTime);
        if (existing.Any())
            throw new InvalidOperationException("Já existe um horário cadastrado para essa data e hora.");

        var slot = new AvailableSlot
        {
            DateTime = dateTime,
            IsActive = true,
            IsBooked = false
        };

        return await _slotRepository.AddAsync(slot);
    }

    public async Task ToggleSlotAsync(int slotId)
    {
        var slot = await _slotRepository.GetByIdAsync(slotId);
        if (slot == null) return;

        slot.IsActive = !slot.IsActive;
        await _slotRepository.UpdateAsync(slot);
    }

    public async Task DeleteSlotAsync(int slotId)
    {
        var slot = await _slotRepository.GetByIdAsync(slotId);
        if (slot == null) return;

        if (slot.IsBooked)
        {
            var appointments = await _appointmentRepository.FindAsync(
                a => a.AvailableSlotId == slotId);

            foreach (var appointment in appointments)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.InternalNotes = "Agendamento cancelado automaticamente — horário excluído pelo administrador.";
                await _appointmentRepository.UpdateAsync(appointment);
            }
        }

        await _slotRepository.DeleteAsync(slot);
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        var slot = await _slotRepository.GetByIdAsync(appointment.AvailableSlotId);
        if (slot == null || !slot.IsActive || slot.IsBooked)
            throw new InvalidOperationException("Este horário não está mais disponível.");

        slot.IsBooked = true;
        await _slotRepository.UpdateAsync(slot);

        appointment.Status = AppointmentStatus.Pending;
        return await _appointmentRepository.AddAsync(appointment);
    }
}