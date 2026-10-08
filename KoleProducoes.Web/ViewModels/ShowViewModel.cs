using KoleProducoes.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KoleProducoes.Web.ViewModels;

public class ShowViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do artista")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string ArtistName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o nome do show")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string ShowName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a data e hora")]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Informe o local")]
    public string Venue { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a cidade")]
    public string City { get; set; } = string.Empty;

    [Range(0, 999999.99, ErrorMessage = "Preço inválido")]
    public decimal? StartingPrice { get; set; }

    public string? Description { get; set; }
    public string? TicketsLink { get; set; }
    public ShowStatus Status { get; set; } = ShowStatus.Draft;
    public int? Capacity { get; set; }

    [Required(ErrorMessage = "Selecione uma categoria")]
    public int CategoryId { get; set; }

    // List of categories for the form dropdown
    // SelectListItem is the type Razor uses to generate <option> elements within a <select> element
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}