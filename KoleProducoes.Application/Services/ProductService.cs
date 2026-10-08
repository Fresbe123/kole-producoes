using KoleProducoes.Application.Interfaces;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace KoleProducoes.Application.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _productRepository.GetAllAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _productRepository.GetByIdAsync(id);

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        var products = await _productRepository.FindAsync(p => p.Slug == slug);
        return products.FirstOrDefault();
    }

    public async Task<IEnumerable<Product>> GetAvailableAsync()
        => await _productRepository.FindAsync(p => p.IsAvailable && p.IsActive);

    public async Task<Product> CreateAsync(Product product)
    {
        product.Slug = await GenerateUniqueSlugAsync(product.Name);
        return await _productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        if (string.IsNullOrEmpty(product.Slug))
            product.Slug = await GenerateUniqueSlugAsync(product.Name);

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
            await _productRepository.DeleteAsync(product);
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

        while ((await _productRepository.FindAsync(p => p.Slug == slug)).Any())
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}