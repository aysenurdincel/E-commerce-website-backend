
using ECommerAPI.Domain.Entities;
using ECommerAPI.Domain.Entities.Common;
using ECommerAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Context
{
    public class ECommerceAPIDbContext : IdentityDbContext<User,UserRole,string>
    {
        public ECommerceAPIDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        //TPH(TABLE PER HIERARCHY)
        public DbSet<SiteFile> Files { get; set; }
        public DbSet<ProductImageFile> ProductImages { get; set; }
        public DbSet<InvoiceFile> Invoices { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //change tracker is a property that catches the changes done through entities.
            //catches the tracked values in update operations
            var data = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in data)
            {
                //won't return anything so discard
                _ = entry.State switch
                {
                    EntityState.Added => entry.Entity.CreationDate = DateTime.UtcNow,
                    EntityState.Modified => entry.Entity.LastModifiedDate = DateTime.UtcNow,

                    //if entity is deleted use this state.
                    _ => DateTime.UtcNow,
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
