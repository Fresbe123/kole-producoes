using KoleProducoes.Application.Models;

namespace KoleProducoes.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardData> GetDashboardDataAsync();
}