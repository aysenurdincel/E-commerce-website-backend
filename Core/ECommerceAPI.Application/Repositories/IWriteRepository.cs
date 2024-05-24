using ECommerAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        Task<bool> DeleteById(string id);

        Task<bool> AddRangeAsync(List<T> entities);
        bool Delete(List<T> entities);
        Task<int> SaveAsync();
    }
}
