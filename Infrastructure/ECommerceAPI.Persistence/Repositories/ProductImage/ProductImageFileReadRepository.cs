using ECommerAPI.Domain.Entities;
using ECommerceAPI.Application.Repositories.ProductImage;
using ECommerceAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories.ProductImage
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
