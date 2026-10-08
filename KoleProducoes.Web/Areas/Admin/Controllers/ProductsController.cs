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
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IRepository<Category> _categoryRepository;

    public ProductsController(
        IProductService productService,
        IRepository<Category> categoryRepository)
    {
        _productService = productService;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Produtos";
        ViewData["ActiveMenu"] = "products";

        var products = await _productService.GetAllAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Novo Produto";
        ViewData["ActiveMenu"] = "products";

        var model = new ProductViewModel
        {
            Categories = await GetCategoriesSelectListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await GetCategoriesSelectListAsync();
            return View(model);
        }

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            DailyRate = model.DailyRate,
            StockQuantity = model.StockQuantity,
            IsAvailable = model.IsAvailable,
            IsActive = model.IsActive,
            CategoryId = model.CategoryId
        };

        await _productService.CreateAsync(product);

        TempData["Success"] = "Produto criado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Produto";
        ViewData["ActiveMenu"] = "products";

        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        var model = new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            DailyRate = product.DailyRate,
            StockQuantity = product.StockQuantity,
            IsAvailable = product.IsAvailable,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            Categories = await GetCategoriesSelectListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await GetCategoriesSelectListAsync();
            return View(model);
        }

        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        product.Name = model.Name;
        product.Description = model.Description;
        product.DailyRate = model.DailyRate;
        product.StockQuantity = model.StockQuantity;
        product.IsAvailable = model.IsAvailable;
        product.IsActive = model.IsActive;
        product.CategoryId = model.CategoryId;

        await _productService.UpdateAsync(product);

        TempData["Success"] = "Produto atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        TempData["Success"] = "Produto excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync()
    {
        // Filtra apenas categorias do tipo Produto
        var categories = await _categoryRepository.FindAsync(
            c => c.Type == CategoryType.Product && c.IsActive);

        return categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        });
    }
}