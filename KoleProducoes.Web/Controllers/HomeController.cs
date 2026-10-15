using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Controllers;

public class HomeController : Controller
{
    private readonly IShowService _showService;
    private readonly IAppointmentService _appointmentService;

    public HomeController(
        IShowService showService,
        IAppointmentService appointmentService)
    {
        _showService = showService;
        _appointmentService = appointmentService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Kole Produções";

        var allShows = await _showService.GetAllAsync();
        var slots = await _appointmentService.GetAvailableSlotsAsync();

        var model = new HomeViewModel
        {
            UpcomingShows = allShows
                .Where(s => s.DateTime >= DateTime.Now &&
                           (s.Status == ShowStatus.Published ||
                            s.Status == ShowStatus.Featured))
                .OrderBy(s => s.DateTime)
                .Take(6),

            PastShows = allShows
                .Where(s => s.DateTime < DateTime.Now || s.Status == ShowStatus.Closed)
                .OrderByDescending(s => s.DateTime)
                .Take(5),

            AvailableSlots = slots
                .OrderBy(s => s.DateTime)
                .Take(9)
        };

        return View(model);
    }
}