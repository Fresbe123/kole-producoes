using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoleProducoes.Domain.Entities;

public class DJ : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public string? MusicStyle { get; set; }
    public decimal? StartingPrice { get; set; }
    public string? PhotoUrl { get; set; }
    public string? SoundCloudLink { get; set; }
    public string? YouTubeLink { get; set; }
    public string? InstagramLink { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<DJAvailability> Availabilities { get; set; } = new List<DJAvailability>();
}

public class DJAvailability : BaseEntity
{
    public int DJId { get; set; }
    public DJ DJ { get; set; } = null!;
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; } = true;
}
