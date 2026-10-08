using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class DJsController : Controller
{
    private readonly IDJService _djService;

    public DJsController(IDJService djService)
    {
        _djService = djService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "DJs";
        ViewData["ActiveMenu"] = "djs";

        var djs = await _djService.GetAllAsync();
        return View(djs);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Novo DJ";
        ViewData["ActiveMenu"] = "djs";
        return View(new DJViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DJViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dj = new DJ
        {
            Name = model.Name,
            Biography = model.Biography,
            MusicStyle = model.MusicStyle,
            StartingPrice = model.StartingPrice,
            PhotoUrl = model.PhotoUrl,
            SoundCloudLink = model.SoundCloudLink,
            YouTubeLink = model.YouTubeLink,
            InstagramLink = model.InstagramLink,
            IsActive = model.IsActive
        };

        await _djService.CreateAsync(dj);

        TempData["Success"] = "DJ cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar DJ";
        ViewData["ActiveMenu"] = "djs";

        var dj = await _djService.GetByIdAsync(id);
        if (dj == null) return NotFound();

        var model = new DJViewModel
        {
            Id = dj.Id,
            Name = dj.Name,
            Biography = dj.Biography,
            MusicStyle = dj.MusicStyle,
            StartingPrice = dj.StartingPrice,
            PhotoUrl = dj.PhotoUrl,
            SoundCloudLink = dj.SoundCloudLink,
            YouTubeLink = dj.YouTubeLink,
            InstagramLink = dj.InstagramLink,
            IsActive = dj.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DJViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dj = await _djService.GetByIdAsync(id);
        if (dj == null) return NotFound();

        dj.Name = model.Name;
        dj.Biography = model.Biography;
        dj.MusicStyle = model.MusicStyle;
        dj.StartingPrice = model.StartingPrice;
        dj.PhotoUrl = model.PhotoUrl;
        dj.SoundCloudLink = model.SoundCloudLink;
        dj.YouTubeLink = model.YouTubeLink;
        dj.InstagramLink = model.InstagramLink;
        dj.IsActive = model.IsActive;

        await _djService.UpdateAsync(dj);

        TempData["Success"] = "DJ atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        await _djService.DeleteAsync(id);
        TempData["Success"] = "DJ excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}