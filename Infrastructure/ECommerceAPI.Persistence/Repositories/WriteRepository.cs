using ECommerAPI.Domain.Entities.Common;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly ECommerceAPIDbContext _context;

        public WriteRepository(ECommerceAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entry = await Table.AddAsync(entity);
            return entry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return true;
        }

        public bool Delete(T entity)
        {
            EntityEntry<T> entry = Table.Remove(entity);
            return entry.State == EntityState.Deleted;
        }

        public bool Delete(List<T> entities)
        {
            Table.RemoveRange(entities);
            return true;
        }

        public bool Update(T entity)
        {
            EntityEntry entry = Table.Update(entity);
            return entry.State == EntityState.Modified;
        }

        public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();

        public async Task<bool> DeleteById(string id)
        {
            T data = await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return Delete(data);
        }
    }
       
}
