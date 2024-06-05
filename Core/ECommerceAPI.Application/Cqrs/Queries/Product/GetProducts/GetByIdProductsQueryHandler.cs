using ECommerceAPI.Application.Cqrs.Queries.Product.GetProducts;
using ECommerceAPI.Application.Repositories;
using MediatR;
using P = ECommerAPI.Domain.Entities;

namespace ECommerceAPI.Application.Cqrs.Queries.GetProducts
{
    public class GetByIdProductsQueryHandler : IRequestHandler<GetByIdProductsQueryRequest, GetByIdProductsQueryResponse>
    { 
        IProductReadRepository _productReadRepository;

        public GetByIdProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetByIdProductsQueryResponse> Handle(GetByIdProductsQueryRequest request, CancellationToken cancellationToken)
        {
            P.Product product = await _productReadRepository.GetByIdAsync(request.Id, false);
            return new()
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };
        }
    }
}
