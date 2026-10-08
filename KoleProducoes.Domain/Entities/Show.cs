using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoleProducoes.Domain.Entities
{
    public enum ShowStatus
    {
        Draft = 1,
        Published = 2,
        Closed = 3,
        Featured = 4,   
    }

    public class Show : BaseEntity
    {
        public string ArtistName { get; set; } = string.Empty;
        public string ShowName { get; set; } = string.Empty;
        public string Slug {  get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Venue {  get; set; } = string.Empty;
        public string City {  get; set; } = string.Empty;  
        public decimal? StartingPrice { get; set; }
        public string? Description {  get; set; } 
        public string? TicketsLink { get; set; }
        public ShowStatus Status { get; set; } = ShowStatus.Draft;
        public int? Capacity { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ShowImage> Images { get; set; } = new List<ShowImage>(); 
    }
}
