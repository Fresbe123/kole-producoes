using KoleProducoes.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KoleProducoes.Web.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do produto")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Máximo 2000 caracteres")]
    public string? Description { get; set; }

    [Range(0, 999999.99, ErrorMessage = "Valor inválido")]
    public decimal? DailyRate { get; set; }

    [Range(0, 9999, ErrorMessage = "Quantidade inválida")]
    public int StockQuantity { get; set; } = 0;

    public bool IsAvailable { get; set; } = true;
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Selecione uma categoria")]
    public int CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}