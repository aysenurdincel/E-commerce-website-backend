using ECommerAPI.Domain.Entities;
using P = ECommerAPI.Domain.Entities;
using MediatR;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.ProductImage;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Cqrs.Commands.ProductImage.DeleteImages
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        IProductReadRepository _productReadRepository;
        IProductImageFileWriteRepository _productImageFileWriteRepository;

        public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            P.Product? product = await _productReadRepository.Table.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));
            ProductImageFile? productImageFile = product?.ProductImages.FirstOrDefault(x => x.Id == Guid.Parse(request.ImageId));

            if(productImageFile != null)
            {
                product?.ProductImages.Remove(productImageFile);
            }
            
            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
