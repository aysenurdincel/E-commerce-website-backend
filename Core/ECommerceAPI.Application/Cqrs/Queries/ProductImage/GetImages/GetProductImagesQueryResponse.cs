using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Queries.ProductImage
{
    public class GetProductImagesQueryResponse
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
