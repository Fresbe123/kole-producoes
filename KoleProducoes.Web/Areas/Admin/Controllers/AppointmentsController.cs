using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AppointmentsController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Agendamentos";
        ViewData["ActiveMenu"] = "appointments";

        var appointments = await _appointmentService.GetAllAsync();
        return View(appointments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalhes do Agendamento";
        ViewData["ActiveMenu"] = "appointments";

        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null) return NotFound();

        return View(appointment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(UpdateAppointmentStatusViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Details), new { id = model.Id });

        await _appointmentService.UpdateStatusAsync(model.Id, model.Status, model.InternalNotes);

        TempData["Success"] = "Status atualizado com sucesso!";
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Slots()
    {
        ViewData["Title"] = "Horários Disponíveis";
        ViewData["ActiveMenu"] = "appointments";

        var slots = await _appointmentService.GetAllSlotsAsync();

        var grouped = slots
            .OrderBy(s => s.DateTime)
            .GroupBy(s => s.DateTime.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        ViewData["GroupedSlots"] = grouped;
        return View(slots);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateSlot(CreateSlotViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Data e hora inválidas.";
            return RedirectToAction(nameof(Slots));
        }

        try
        {
            await _appointmentService.CreateSlotAsync(model.DateTime);
            TempData["Success"] = "Horário criado com sucesso!";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Slots));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleSlot(int id)
    {
        await _appointmentService.ToggleSlotAsync(id);
        return RedirectToAction(nameof(Slots));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteSlot(int id)
    {
        try
        {
            await _appointmentService.DeleteSlotAsync(id);
            TempData["Success"] = "Horário excluído com sucesso!";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Slots));
    }
}