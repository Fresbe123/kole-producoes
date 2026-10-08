using System.ComponentModel.DataAnnotations;

namespace KoleProducoes.Web.ViewModels;

public class UserViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Informe o nome completo")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecione o perfil")]
    public string Role { get; set; } = "Editor";

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas não conferem")]
    public string? ConfirmPassword { get; set; }

    public string? CurrentRole { get; set; }
}