using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace KoleProducoes.Application.Services;

public class DJService : IDJService
{
    private readonly IRepository<DJ> _djRepository;

    public DJService(IRepository<DJ> djRepository)
    {
        _djRepository = djRepository;
    }

    public async Task<IEnumerable<DJ>> GetAllAsync()
        => await _djRepository.GetAllAsync();

    public async Task<DJ?> GetByIdAsync(int id)
        => await _djRepository.GetByIdAsync(id);

    public async Task<DJ?> GetBySlugAsync(string slug)
    {
        var djs = await _djRepository.FindAsync(d => d.Slug == slug);
        return djs.FirstOrDefault();
    }

    public async Task<IEnumerable<DJ>> GetActiveAsync()
        => await _djRepository.FindAsync(d => d.IsActive);

    public async Task<DJ> CreateAsync(DJ dj)
    {
        dj.Slug = await GenerateUniqueSlugAsync(dj.Name);
        return await _djRepository.AddAsync(dj);
    }

    public async Task UpdateAsync(DJ dj)
    {
        if (string.IsNullOrEmpty(dj.Slug))
            dj.Slug = await GenerateUniqueSlugAsync(dj.Name);

        await _djRepository.UpdateAsync(dj);
    }

    public async Task DeleteAsync(int id)
    {
        var dj = await _djRepository.GetByIdAsync(id);
        if (dj != null)
            await _djRepository.DeleteAsync(dj);
    }

    public string GenerateSlug(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");

        return slug.Trim('-');
    }

    private async Task<string> GenerateUniqueSlugAsync(string text)
    {
        var baseSlug = GenerateSlug(text);
        var slug = baseSlug;
        var counter = 1;

        while ((await _djRepository.FindAsync(d => d.Slug == slug)).Any())
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}