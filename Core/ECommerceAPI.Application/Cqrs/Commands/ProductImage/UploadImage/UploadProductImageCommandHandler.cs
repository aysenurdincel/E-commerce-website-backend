using P = ECommerAPI.Domain.Entities;
using MediatR;
using ECommerceAPI.Application.Storages;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.ProductImage;
using ECommerAPI.Domain.Entities;

namespace ECommerceAPI.Application.Cqrs.Commands.ProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        IStorageService _storageService;
        IProductReadRepository _productReadRepository;
        IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _storageService = storageService;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string filePath)> data = await _storageService.UploadAsync("product-images",request.FormFileCollection);

            P.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            await _productImageFileWriteRepository.AddRangeAsync(data.Select(
                x => new ProductImageFile
                {
                    Name = x.fileName,
                    Path = x.filePath,
                    Storage = _storageService.StorageName,
                    Products = new List<P.Product> { product }
                }).ToList());
            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
