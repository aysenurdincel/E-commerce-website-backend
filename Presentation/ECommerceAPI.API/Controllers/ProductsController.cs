
using ECommerAPI.Domain.Entities;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Invoice;
using ECommerceAPI.Application.Repositories.ProductImage;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.Storages;
using ECommerceAPI.Application.ViewModels.Products;
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
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IConfiguration _configuration;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IStorageService storageService, ISiteFileWriteRepository siteFileWriteRepository, ISiteFileReadRepository siteFileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IConfiguration configuration)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService = storageService;
            _siteFileWriteRepository = siteFileWriteRepository;
            _siteFileReadRepository = siteFileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Pagination pagination)
        {
            //pagination için total data sayısını bildir
            var total = _productReadRepository.GetAll(false).Count();

            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Name,
                p.Stock,
                p.Price,
                p.Id,
                p.CreationDate,
                p.LastModifiedDate
            }).ToList();

            return Ok(new { total, products });
        }

        //farklı türde dosylar gelirse video vb. yönetilebilir olması için querystring
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok( _productReadRepository.GetByIdAsync(id, false));
        }

        //kesin image olduğu için route da tanımlı
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return Ok(product.ProductImages.Select(x => new
            {
                Path = $"{_configuration["BaseLocalStorageUrl"]}\\{x.Path}",
                x.Name,
                x.Id
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_ProductCreate product)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price
            });
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName,string filePath)> data = await _storageService.UploadAsync("product-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(data.Select(
                x=> new ProductImageFile { 
                    Name = x.fileName,
                    Path = x.filePath,
                    Storage = _storageService.StorageName,
                    Products = new List<Product> { product }
                }).ToList());
            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_ProductUpdate model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name = model.Name;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) 
        {
            await _productWriteRepository.DeleteById(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            ProductImageFile productImageFile = product.ProductImages.FirstOrDefault(x => x.Id == Guid.Parse(imageId));
            product.ProductImages.Remove(productImageFile);
            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }


    }
}
