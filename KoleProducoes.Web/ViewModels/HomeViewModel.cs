using KoleProducoes.Domain.Entities;

namespace KoleProducoes.Web.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Show> UpcomingShows { get; set; } = new List<Show>();
    public IEnumerable<Show> PastShows { get; set; } = new List<Show>();
    public IEnumerable<AvailableSlot> AvailableSlots { get; set; } = new List<AvailableSlot>();
}