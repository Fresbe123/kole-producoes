using KoleProducoes.Application.Interfaces;
using KoleProducoes.Application.Models;
using KoleProducoes.Domain.Entities;
using KoleProducoes.Domain.Interfaces;

namespace KoleProducoes.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IRepository<Show> _showRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<DJ> _djRepository;
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<Category> _categoryRepository;

    public DashboardService(
        IRepository<Show> showRepository,
        IRepository<Product> productRepository,
        IRepository<DJ> djRepository,
        IRepository<Appointment> appointmentRepository,
        IRepository<Category> categoryRepository)
    {
        _showRepository = showRepository;
        _productRepository = productRepository;
        _djRepository = djRepository;
        _appointmentRepository = appointmentRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<DashboardData> GetDashboardDataAsync()
    {
        var shows = (await _showRepository.GetAllAsync()).ToList();
        var products = (await _productRepository.GetAllAsync()).ToList();
        var djs = (await _djRepository.GetAllAsync()).ToList();
        var appointments = (await _appointmentRepository.GetAllAsync()).ToList();
        var categories = (await _categoryRepository.GetAllAsync()).ToList();

        var recentAppointments = appointments
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => new RecentAppointmentData
            {
                Id = a.Id,
                ClientName = a.ClientName,
                Email = a.Email,
                RequestedDateTime = a.RequestedDateTime,
                Status = GetAppointmentStatusLabel(a.Status),
                StatusClass = GetAppointmentStatusClass(a.Status)
            });

        var upcomingShows = shows
            .Where(s => s.DateTime >= DateTime.Now &&
                       (s.Status == ShowStatus.Published || s.Status == ShowStatus.Featured))
            .OrderBy(s => s.DateTime)
            .Take(5)
            .Select(s => new UpcomingShowData
            {
                Id = s.Id,
                ArtistName = s.ArtistName,
                ShowName = s.ShowName,
                DateTime = s.DateTime,
                City = s.City,
                Status = GetShowStatusLabel(s.Status),
                StatusClass = GetShowStatusClass(s.Status)
            });

        return new DashboardData
        {
            TotalShows = shows.Count,
            PublishedShows = shows.Count(s =>
                s.Status == ShowStatus.Published ||
                s.Status == ShowStatus.Featured),
            TotalProducts = products.Count,
            TotalDJs = djs.Count(d => d.IsActive),
            PendingAppointments = appointments.Count(a => a.Status == AppointmentStatus.Pending),
            TotalAppointments = appointments.Count,
            TotalCategories = categories.Count(c => c.IsActive),
            RecentAppointments = recentAppointments,
            UpcomingShows = upcomingShows
        };
    }

    private static string GetAppointmentStatusLabel(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Pending => "Pendente",
        AppointmentStatus.Confirmed => "Confirmado",
        AppointmentStatus.Cancelled => "Cancelado",
        AppointmentStatus.Completed => "Realizado",
        _ => ""
    };

    private static string GetAppointmentStatusClass(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Pending => "badge-yellow",
        AppointmentStatus.Confirmed => "badge-green",
        AppointmentStatus.Cancelled => "badge-red",
        AppointmentStatus.Completed => "badge-gray",
        _ => ""
    };

    private static string GetShowStatusLabel(ShowStatus status) => status switch
    {
        ShowStatus.Published => "Publicado",
        ShowStatus.Featured => "Destacado",
        ShowStatus.Draft => "Rascunho",
        ShowStatus.Closed => "Encerrado",
        _ => ""
    };

    private static string GetShowStatusClass(ShowStatus status) => status switch
    {
        ShowStatus.Published => "badge-green",
        ShowStatus.Featured => "badge-green",
        ShowStatus.Draft => "badge-gray",
        ShowStatus.Closed => "badge-yellow",
        _ => ""
    };
}