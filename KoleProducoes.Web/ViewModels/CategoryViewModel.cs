using KoleProducoes.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace KoleProducoes.Web.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome da categoria")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Selecione o tipo")]
    public CategoryType Type { get; set; }

    public bool IsActive { get; set; } = true;

    public int Order { get; set; } = 0;
}