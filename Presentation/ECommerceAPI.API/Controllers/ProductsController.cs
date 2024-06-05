
using ECommerAPI.Domain.Entities;
using ECommerceAPI.Application.Cqrs.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Cqrs.Commands.Product.DeleteProduct;
using ECommerceAPI.Application.Cqrs.Commands.Product.UpdateProduct;
using ECommerceAPI.Application.Cqrs.Commands.ProductImage;
using ECommerceAPI.Application.Cqrs.Commands.ProductImage.DeleteImages;
using ECommerceAPI.Application.Cqrs.Queries.GetProducts;
using ECommerceAPI.Application.Cqrs.Queries.Product.GetProducts;
using ECommerceAPI.Application.Cqrs.Queries.ProductImage;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.ProductImage;
using ECommerceAPI.Application.Storages;
using ECommerceAPI.Application.ViewModels.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //TEST CONTROLLER
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IStorageService _storageService;
        private readonly ISiteFileWriteRepository _siteFileWriteRepository;
        private readonly ISiteFileReadRepository _siteFileReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IStorageService storageService, ISiteFileWriteRepository siteFileWriteRepository, ISiteFileReadRepository siteFileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IConfiguration configuration, IMediator mediator)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService = storageService;
            _siteFileWriteRepository = siteFileWriteRepository;
            _siteFileReadRepository = siteFileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetAllProductsQueryRequest request)
        {
            GetAllProductsQueryResponse response =  await _mediator.Send(request);
            return Ok(response);
        }

        //farklı türde dosylar gelirse video vb. yönetilebilir olması için querystring
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute]GetByIdProductsQueryRequest request)
        {
            GetByIdProductsQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        //kesin image olduğu için route da tanımlı
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetImages([FromRoute]GetProductImagesQueryRequest request)
        {
            List<GetProductImagesQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest request)
        {
            CreateProductCommandResponse reponse = await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest request)
        {
            request.FormFileCollection = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest request)
        {
            UpdateProductCommandResponse response = await _mediator.Send(request);
            return Ok();
        }
        
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest request) 
        {
            DeleteProductCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteImage([FromRoute]DeleteProductImageCommandRequest request, [FromQuery] string ImageId)
        {
            request.ImageId = ImageId;
            DeleteProductImageCommandResponse response = await _mediator.Send(request);
            return Ok();
        }


    }
}
