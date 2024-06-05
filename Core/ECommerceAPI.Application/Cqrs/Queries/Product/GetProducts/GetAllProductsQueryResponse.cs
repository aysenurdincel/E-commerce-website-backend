using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Queries.GetProducts
{
    public class GetAllProductsQueryResponse
    {
        public int  Total { get; set; }
        public object Products { get; set; }
    }
}
