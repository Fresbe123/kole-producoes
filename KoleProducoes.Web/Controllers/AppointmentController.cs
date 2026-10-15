using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home", null, "agendamento");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        string clientName, string email, string phone,
        int interestType, string? message, int availableSlotId)
    {
        if (string.IsNullOrEmpty(clientName) || string.IsNullOrEmpty(email) || availableSlotId == 0)
        {
            TempData["AppointmentError"] = "Preencha todos os campos e selecione um horário.";
            return RedirectToAction("Index", "Home", null, "agendamento");
        }

        try
        {
            var appointment = new Appointment
            {
                ClientName = clientName,
                Email = email,
                Phone = phone,
                InterestType = (InterestType)interestType,
                Message = message,
                AvailableSlotId = availableSlotId,
                RequestedDateTime = DateTime.Now,
                Status = AppointmentStatus.Pending
            };

            await _appointmentService.CreateAppointmentAsync(appointment);
            TempData["AppointmentSuccess"] = "true";
        }
        catch (InvalidOperationException)
        {
            TempData["AppointmentError"] = "Este horário não está mais disponível. Escolha outro.";
        }

        return RedirectToAction("Index", "Home", null, "agendamento");
    }
}