using KoleProducoes.Application.Interfaces;
using KoleProducoes.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace KoleProducoes.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        => _userManager.Users.ToList();

    public async Task<ApplicationUser?> GetByIdAsync(string id)
        => await _userManager.FindByIdAsync(id);

    public async Task<string> GetRoleAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.FirstOrDefault() ?? "Editor";
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> CreateAsync(
        ApplicationUser user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, role);
        return (true, Enumerable.Empty<string>());
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> UpdateAsync(
        ApplicationUser user, string? newPassword, string role)
    {
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);

        if (!string.IsNullOrEmpty(newPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!passwordResult.Succeeded)
                return (false, passwordResult.Errors.Select(e => e.Description));
        }

        return (true, Enumerable.Empty<string>());
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return (false, new[] { "Usuário não encontrado." });

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        return (true, Enumerable.Empty<string>());
    }
}