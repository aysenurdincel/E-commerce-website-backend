using ECommerAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; } //ayrı ayrı tutmaya karar verirsen şehir ülke vs value objecte çevir
        public ICollection<Product> Products { get; set; }
        public Customer Customer { get; set; }
    }
}
