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
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
    {
        public CustomerWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
