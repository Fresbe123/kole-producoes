using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace KoleProducoes.Application.Services;

public class ShowService : IShowService
{
    private readonly IRepository<Show> _showRepository;

    public ShowService(IRepository<Show> showRepository)
    {
        _showRepository = showRepository;
    }

    public async Task<IEnumerable<Show>> GetAllAsync()
    => await _showRepository.GetAllWithIncludesAsync(
        s => s.Category,
        s => s.Images);

    public async Task<Show?> GetByIdAsync(int id)
        => await _showRepository.GetByIdAsync(id);

    public async Task<Show?> GetBySlugAsync(string slug)
    {
        var shows = await _showRepository.FindAsync(s => s.Slug == slug);
        return shows.FirstOrDefault();
    }

    public async Task<IEnumerable<Show>> GetPublishedAsync()
    {
        return await _showRepository.FindAsync(s =>
            s.Status == ShowStatus.Published ||
            s.Status == ShowStatus.Featured);
    }

    public async Task<Show> CreateAsync(Show show)
    {
        show.Slug = await GenerateUniqueSlugAsync(show.ArtistName + " " + show.ShowName);
        return await _showRepository.AddAsync(show);
    }

    public async Task UpdateAsync(Show show)
    {
        if (string.IsNullOrEmpty(show.Slug))
            show.Slug = await GenerateUniqueSlugAsync(show.ArtistName + " " + show.ShowName);

        await _showRepository.UpdateAsync(show);
    }

    public async Task DeleteAsync(int id)
    {
        var show = await _showRepository.GetByIdAsync(id);
        if (show != null)
            await _showRepository.DeleteAsync(show);
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

        while ((await _showRepository.FindAsync(s => s.Slug == slug)).Any())
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}