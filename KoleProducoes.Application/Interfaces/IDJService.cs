using KoleProducoes.Domain.Entities;

namespace KoleProducoes.Application.Interfaces;

public interface IDJService
{
    Task<IEnumerable<DJ>> GetAllAsync();
    Task<DJ?> GetByIdAsync(int id);
    Task<DJ?> GetBySlugAsync(string slug);
    Task<IEnumerable<DJ>> GetActiveAsync();
    Task<DJ> CreateAsync(DJ dj);
    Task UpdateAsync(DJ dj);
    Task DeleteAsync(int id);
    string GenerateSlug(string text);
}