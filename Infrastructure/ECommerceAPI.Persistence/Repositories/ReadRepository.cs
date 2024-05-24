using ECommerAPI.Domain.Entities.Common;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommerceAPIDbContext _context;

        public ReadRepository(ECommerceAPIDbContext context)
        {
            _context = context;
        }

        //retrieve DbSet
        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }return query;
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        { 
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
        }

        public async Task<T> GetValueAsync(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if(!tracking)
            {
                query = query.AsNoTracking();
            }return await query.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            var query = Table.Where(predicate);
            if (!tracking)
            {
                query = query.AsNoTracking();
            }return query;
        }
    }
}
