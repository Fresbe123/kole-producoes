using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CategoriesController : Controller
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoriesController(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Categorias";
        ViewData["ActiveMenu"] = "categories";

        // Busca todas e ordena pela propriedade Order
        var categories = await _categoryRepository.GetAllAsync();
        return View(categories.OrderBy(c => c.Order));
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Nova Categoria";
        ViewData["ActiveMenu"] = "categories";
        return View(new CategoryViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var category = new Category
        {
            Name = model.Name,
            Description = model.Description,
            Type = model.Type,
            IsActive = model.IsActive,
            Order = model.Order
        };

        await _categoryRepository.AddAsync(category);

        TempData["Success"] = "Categoria criada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Categoria";
        ViewData["ActiveMenu"] = "categories";

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();

        var model = new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Type = category.Type,
            IsActive = category.IsActive,
            Order = category.Order
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int id, CategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return NotFound();

        category.Name = model.Name;
        category.Description = model.Description;
        category.Type = model.Type;
        category.IsActive = model.IsActive;
        category.Order = model.Order;

        await _categoryRepository.UpdateAsync(category);

        TempData["Success"] = "Categoria atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepository.GetByIdWithIncludesAsync(
            id,
            c => c.Shows,
            c => c.Products
        );

        if (category == null) return NotFound();

        if (category.Shows.Any() || category.Products.Any())
        {
            TempData["Error"] = "Não é possível excluir uma categoria com shows ou produtos vinculados.";
            return RedirectToAction(nameof(Index));
        }

        await _categoryRepository.DeleteAsync(category);

        TempData["Success"] = "Categoria excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}