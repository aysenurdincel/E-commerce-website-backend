using ECommerceAPI.Application.Cqrs.Queries.Product.GetProducts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Queries.GetProducts
{
    public class GetByIdProductsQueryRequest : IRequest<GetByIdProductsQueryResponse>
    {
        public string Id { get; set; }
    }
}
