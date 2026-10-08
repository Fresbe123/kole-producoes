using KoleProducoes.Domain.Entities;

namespace KoleProducoes.Application.Interfaces;

public interface IAppointmentService
{
    // Appointments
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<Appointment?> GetByIdAsync(int id);
    Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status);
    Task<int> GetPendingCountAsync();
    Task UpdateStatusAsync(int id, AppointmentStatus status, string? internalNotes);

    // Available Slots
    Task<IEnumerable<AvailableSlot>> GetAllSlotsAsync();
    Task<IEnumerable<AvailableSlot>> GetAvailableSlotsAsync();
    Task<AvailableSlot> CreateSlotAsync(DateTime dateTime);
    Task ToggleSlotAsync(int slotId);
    Task DeleteSlotAsync(int slotId);

    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
}