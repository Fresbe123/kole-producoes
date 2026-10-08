namespace KoleProducoes.Application.Models;

public class DashboardData
{
    public int TotalShows { get; set; }
    public int PublishedShows { get; set; }
    public int TotalProducts { get; set; }
    public int TotalDJs { get; set; }
    public int PendingAppointments { get; set; }
    public int TotalAppointments { get; set; }
    public int TotalCategories { get; set; }
    public IEnumerable<RecentAppointmentData> RecentAppointments { get; set; } = new List<RecentAppointmentData>();
    public IEnumerable<UpcomingShowData> UpcomingShows { get; set; } = new List<UpcomingShowData>();
}

public class RecentAppointmentData
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RequestedDateTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
}

public class UpcomingShowData
{
    public int Id { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public string ShowName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string City { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;
}