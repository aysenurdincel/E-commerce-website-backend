using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerAPI.Domain.Entities
{
    public class ProductImageFile : SiteFile
    {
        public ICollection<Product> Products { get; set; }
    }
}
