using KoleProducoes.Application.Interfaces;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        ViewData["ActiveMenu"] = "dashboard";

        var data = await _dashboardService.GetDashboardDataAsync();
        ViewData["PendingAppointments"] = data.PendingAppointments;

        var model = new DashboardViewModel
        {
            TotalShows = data.TotalShows,
            PublishedShows = data.PublishedShows,
            TotalProducts = data.TotalProducts,
            TotalDJs = data.TotalDJs,
            PendingAppointments = data.PendingAppointments,
            TotalAppointments = data.TotalAppointments,
            TotalCategories = data.TotalCategories,
            RecentAppointments = data.RecentAppointments,
            UpcomingShows = data.UpcomingShows
        };

        return View(model);
    }
}