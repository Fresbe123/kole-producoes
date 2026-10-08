using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoleProducoes.Domain.Entities
{
    public enum CategoryType
    {
        Product = 1,
        Service = 2,
        Hiring = 3,
    }

    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CategoryType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public int Order { get; set; } = 0;

        public ICollection<Show> Shows { get; set; } = new List<Show>();    
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
