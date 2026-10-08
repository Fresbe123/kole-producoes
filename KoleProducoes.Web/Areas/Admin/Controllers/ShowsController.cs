using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] 
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

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Shows";
        ViewData["ActiveMenu"] = "shows";

        var shows = await _showService.GetAllAsync();
        return View(shows);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Novo Show";
        ViewData["ActiveMenu"] = "shows";

        var model = new ShowViewModel
        {
            Categories = await GetCategoriesSelectListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShowViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await GetCategoriesSelectListAsync();
            return View(model);
        }

        var show = new Show
        {
            ArtistName = model.ArtistName,
            ShowName = model.ShowName,
            DateTime = model.DateTime,
            Venue = model.Venue,
            City = model.City,
            StartingPrice = model.StartingPrice,
            Description = model.Description,
            TicketsLink = model.TicketsLink,
            Status = model.Status,
            Capacity = model.Capacity,
            CategoryId = model.CategoryId
        };

        await _showService.CreateAsync(show);

        TempData["Success"] = "Show criado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Show";
        ViewData["ActiveMenu"] = "shows";

        var show = await _showService.GetByIdAsync(id);
        if (show == null) return NotFound();

        var model = new ShowViewModel
        {
            Id = show.Id,
            ArtistName = show.ArtistName,
            ShowName = show.ShowName,
            DateTime = show.DateTime,
            Venue = show.Venue,
            City = show.City,
            StartingPrice = show.StartingPrice,
            Description = show.Description,
            TicketsLink = show.TicketsLink,
            Status = show.Status,
            Capacity = show.Capacity,
            CategoryId = show.CategoryId,
            Categories = await GetCategoriesSelectListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ShowViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await GetCategoriesSelectListAsync();
            return View(model);
        }

        var show = await _showService.GetByIdAsync(id);
        if (show == null) return NotFound();

        show.ArtistName = model.ArtistName;
        show.ShowName = model.ShowName;
        show.DateTime = model.DateTime;
        show.Venue = model.Venue;
        show.City = model.City;
        show.StartingPrice = model.StartingPrice;
        show.Description = model.Description;
        show.TicketsLink = model.TicketsLink;
        show.Status = model.Status;
        show.Capacity = model.Capacity;
        show.CategoryId = model.CategoryId;

        await _showService.UpdateAsync(show);

        TempData["Success"] = "Show atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")] 
    public async Task<IActionResult> Delete(int id)
    {
        await _showService.DeleteAsync(id);
        TempData["Success"] = "Show excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        });
    }
}