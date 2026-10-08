using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class HistoryController : Controller
{
    private readonly IRepository<Show> _showRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<DJ> _djRepository;

    public HistoryController(
        IRepository<Show> showRepository,
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IRepository<DJ> djRepository)
    {
        _showRepository = showRepository;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _djRepository = djRepository;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Histórico";
        ViewData["ActiveMenu"] = "history";

        var deletedShows = await _showRepository.GetDeletedAsync();
        var deletedProducts = await _productRepository.GetDeletedAsync();
        var deletedCategories = await _categoryRepository.GetDeletedAsync();
        var deletedDJs = await _djRepository.GetDeletedAsync();

        ViewData["DeletedShows"] = deletedShows.OrderByDescending(s => s.DeletedAt);
        ViewData["DeletedProducts"] = deletedProducts.OrderByDescending(p => p.DeletedAt);
        ViewData["DeletedCategories"] = deletedCategories.OrderByDescending(c => c.DeletedAt);
        ViewData["DeletedDJs"] = deletedDJs.OrderByDescending(d => d.DeletedAt);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreShow(int id)
    {
        await _showRepository.RestoreAsync(id);
        TempData["Success"] = "Show restaurado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreProduct(int id)
    {
        await _productRepository.RestoreAsync(id);
        TempData["Success"] = "Produto restaurado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreCategory(int id)
    {
        await _categoryRepository.RestoreAsync(id);
        TempData["Success"] = "Categoria restaurada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreDJ(int id)
    {
        await _djRepository.RestoreAsync(id);
        TempData["Success"] = "DJ restaurado com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}