using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Controllers;

public class ShowsController : Controller
{
    private readonly IShowService _showService;
    private readonly IRepository<Category> _categoryRepository;

    public ShowsController(
        IShowService showService,
        IRepository<Category> categoryRepository)
    {
        _showService = showService;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index(string? category)
    {
        ViewData["Title"] = "Próximos Shows";

        var allShows = await _showService.GetAllAsync();

        var shows = allShows
            .Where(s => s.DateTime >= DateTime.Now &&
                       (s.Status == ShowStatus.Published ||
                        s.Status == ShowStatus.Featured))
            .OrderBy(s => s.DateTime)
            .ToList();

        if (!string.IsNullOrEmpty(category))
        {
            shows = shows
                .Where(s => s.Category?.Name.ToLower() == category.ToLower())
                .ToList();
        }

        var categories = await _categoryRepository.FindAsync(
            c => c.Type == CategoryType.Service && c.IsActive);

        ViewData["Categories"] = categories;
        ViewData["CurrentCategory"] = category;

        return View(shows);
    }

    public async Task<IActionResult> History()
    {
        ViewData["Title"] = "Histórico de Shows";

        var allShows = await _showService.GetAllAsync();

        var pastShows = allShows
            .Where(s => s.DateTime < DateTime.Now || s.Status == ShowStatus.Closed)
            .OrderByDescending(s => s.DateTime)
            .ToList();

        return View(pastShows);
    }

    public async Task<IActionResult> Details(string slug)
    {
        var show = await _showService.GetBySlugAsync(slug);

        if (show == null) return NotFound();

        if (show.Status == ShowStatus.Draft)
            return NotFound();

        ViewData["Title"] = $"{show.ArtistName} — {show.ShowName}";

        return View(show);
    }
}