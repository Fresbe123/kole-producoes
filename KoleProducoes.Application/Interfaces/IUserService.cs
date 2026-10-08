using KoleProducoes.Infrastructure.Data;

namespace KoleProducoes.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<(bool Success, IEnumerable<string> Errors)> CreateAsync(ApplicationUser user, string password, string role);
    Task<(bool Success, IEnumerable<string> Errors)> UpdateAsync(ApplicationUser user, string? newPassword, string role);
    Task<(bool Success, IEnumerable<string> Errors)> DeleteAsync(string id);
    Task<string> GetRoleAsync(ApplicationUser user);
}