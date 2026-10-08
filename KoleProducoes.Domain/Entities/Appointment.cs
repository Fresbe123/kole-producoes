using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoleProducoes.Domain.Entities;

public enum AppointmentStatus
{
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Completed = 4
}

public enum InterestType
{
    HireShow = 1,
    OrganizeEvent = 2,
    CommercialPartnership = 3,
    Other = 4
}

public class Appointment : BaseEntity
{
    public string ClientName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public InterestType InterestType { get; set; }
    public DateTime RequestedDateTime { get; set; }
    public string? Message { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? InternalNotes { get; set; }

    public int AvailableSlotId { get; set; }
    public AvailableSlot AvailableSlot { get; set; } = null!;
}

public class AvailableSlot : BaseEntity
{
    public DateTime DateTime { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsBooked { get; set; } = false;

    public Appointment? Appointment { get; set; }
}