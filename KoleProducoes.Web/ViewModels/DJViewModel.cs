using System.ComponentModel.DataAnnotations;

namespace KoleProducoes.Web.ViewModels;

public class DJViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do DJ")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Máximo 2000 caracteres")]
    public string? Biography { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? MusicStyle { get; set; }

    [Range(0, 999999.99, ErrorMessage = "Valor inválido")]
    public decimal? StartingPrice { get; set; }

    public string? PhotoUrl { get; set; }

    [Url(ErrorMessage = "Informe uma URL válida")]
    public string? SoundCloudLink { get; set; }

    [Url(ErrorMessage = "Informe uma URL válida")]
    public string? YouTubeLink { get; set; }

    [Url(ErrorMessage = "Informe uma URL válida")]
    public string? InstagramLink { get; set; }

    public bool IsActive { get; set; } = true;
}