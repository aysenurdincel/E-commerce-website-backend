using ECommerAPI.Domain.Entities;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories
{
    public class SiteFileWriteRepository : WriteRepository<SiteFile>, ISiteFileWriteRepository
    {
        public SiteFileWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
