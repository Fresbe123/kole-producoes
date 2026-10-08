using KoleProducoes.Application.Interfaces;
using KoleProducoes.Infrastructure.Data;
using KoleProducoes.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoleProducoes.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")] 
public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Usuários";
        ViewData["ActiveMenu"] = "users";

        var users = await _userService.GetAllAsync();

        var viewModels = new List<UserViewModel>();
        foreach (var user in users)
        {
            viewModels.Add(new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                CurrentRole = await _userService.GetRoleAsync(user)
            });
        }

        return View(viewModels);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Novo Usuário";
        ViewData["ActiveMenu"] = "users";
        return View(new UserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (string.IsNullOrEmpty(model.Password))
            ModelState.AddModelError("Password", "Informe a senha");

        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            FullName = model.FullName,
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        var (success, errors) = await _userService.CreateAsync(user, model.Password!, model.Role);

        if (!success)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        TempData["Success"] = "Usuário criado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["Title"] = "Editar Usuário";
        ViewData["ActiveMenu"] = "users";

        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var model = new UserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? "",
            Role = await _userService.GetRoleAsync(user),
            CurrentRole = await _userService.GetRoleAsync(user)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, UserViewModel model)
    {
        if (!string.IsNullOrEmpty(model.Password) && model.Password != model.ConfirmPassword)
            ModelState.AddModelError("ConfirmPassword", "As senhas não conferem");

        if (!ModelState.IsValid)
            return View(model);

        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;

        var (success, errors) = await _userService.UpdateAsync(user, model.Password, model.Role);

        if (!success)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        TempData["Success"] = "Usuário atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (id == currentUserId)
        {
            TempData["Error"] = "Você não pode excluir sua própria conta.";
            return RedirectToAction(nameof(Index));
        }

        var (success, errors) = await _userService.DeleteAsync(id);

        if (!success)
        {
            TempData["Error"] = errors.FirstOrDefault() ?? "Erro ao excluir usuário.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Usuário excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}