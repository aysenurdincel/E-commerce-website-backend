using P = ECommerAPI.Domain.Entities;
using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerceAPI.Application.Cqrs.Queries.ProductImage
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async  Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            P.Product? product = await _productReadRepository.Table.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));
            return product?.ProductImages.Select(x => new GetProductImagesQueryResponse
            {
                Path = $"{_configuration["BaseLocalStorageUrl"]}\\{x.Path}",
                Name = x.Name,
                Id = x.Id
            }).ToList();
        }
    }
}
