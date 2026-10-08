using KoleProducoes.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace KoleProducoes.Web.ViewModels;

public class AppointmentViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public InterestType InterestType { get; set; }
    public DateTime RequestedDateTime { get; set; }
    public string? Message { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? InternalNotes { get; set; }
    public DateTime SlotDateTime { get; set; }
}

public class UpdateAppointmentStatusViewModel
{
    public int Id { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; }

    public string? InternalNotes { get; set; }
}

public class CreateSlotViewModel
{
    [Required(ErrorMessage = "Informe a data e hora")]
    public DateTime DateTime { get; set; }
}