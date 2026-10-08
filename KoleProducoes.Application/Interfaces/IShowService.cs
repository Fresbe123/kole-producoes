using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoleProducoes.Domain.Entities;

namespace KoleProducoes.Application.Interfaces;

public interface IShowService
{
    Task<IEnumerable<Show>> GetAllAsync();
    Task<Show?> GetByIdAsync(int id);
    Task<Show?> GetBySlugAsync(string slug);
    Task<IEnumerable<Show>> GetPublishedAsync();
    Task<Show> CreateAsync(Show show);
    Task UpdateAsync(Show show);
    Task DeleteAsync(int id);
    string GenerateSlug(string text);
}
