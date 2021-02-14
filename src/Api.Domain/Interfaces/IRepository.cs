using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> InsertAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<bool> DeleteAsync(Guid id);
        Task<T> SelectByIdAsync(Guid id);
        Task<KeyValuePair<int, IEnumerable<T>>> SelectAsync(int currentPage = 1, int perPage = 25, int ordernation = 1, Expression<Func<T, bool>> expression = null);
        Task<bool> ExistAsync(Guid id);
    }
}
