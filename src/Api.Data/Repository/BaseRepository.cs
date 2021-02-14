using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MyContext _context;
        private DbSet<T> _dataset;

        public BaseRepository(MyContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                T target = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(id));
                if (target == null)
                {
                    return false;
                }

                _dataset.Remove(target);
                int processResult = await _context.SaveChangesAsync();

                return processResult > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExistAsync(Guid id) => await _dataset.AnyAsync(p => p.Id.Equals(id));

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                if (item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                }

                item.CreatedAt = DateTime.UtcNow;
                _dataset.Add(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public async Task<T> SelectByIdAsync(Guid id)
        {
            try
            {
                return await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<KeyValuePair<int, IEnumerable<T>>> SelectAscAsync(int currentPage = 1, int perPage = 25, Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                int total = _dataset.OrderByDescending(x => x.CreatedAt).Count();
                List<T> data = await _dataset.OrderBy(x => x.CreatedAt).Skip(currentPage * perPage).Take(perPage).ToListAsync();
                return new KeyValuePair<int, IEnumerable<T>>(total, data);
            }
            else
            {
                int total = _dataset.OrderByDescending(x => x.CreatedAt).Count();
                List<T> data = await _dataset.OrderBy(x => x.CreatedAt).Where(expression).Skip(currentPage * perPage).Take(perPage).ToListAsync();
                return new KeyValuePair<int, IEnumerable<T>>(total, data);
            }
        }

        private async Task<KeyValuePair<int,IEnumerable<T>>> SelectDescAsync(int currentPage = 1, int perPage = 25, Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                int total = _dataset.OrderByDescending(x => x.CreatedAt).Count();
                List<T> data =  await _dataset.OrderByDescending(x => x.CreatedAt).Skip(currentPage * perPage).Take(perPage).ToListAsync();
                return new KeyValuePair<int, IEnumerable<T>>(total, data);
            }
            else
            {
                int total = _dataset.OrderByDescending(x => x.CreatedAt).Count();
                List<T> data = await _dataset.OrderByDescending(x => x.CreatedAt).Where(expression).Skip(currentPage * perPage).Take(perPage).ToListAsync();
                return new KeyValuePair<int, IEnumerable<T>>(total, data);
            }
        }

        public async Task<KeyValuePair<int, IEnumerable<T>>> SelectAsync(int currentPage = 1, int perPage = 25, int ordernation = 1, Expression<Func<T, bool>> expression = null)
        {
            try
            {
                if(ordernation == 1)
                {
                    return await SelectAscAsync(currentPage, perPage, expression);
                }
                else
                {
                    return await SelectDescAsync(currentPage, perPage, expression);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(item.Id));
                if (result == null)
                {
                    return null;
                }

                item.UpdatedAt = DateTime.UtcNow;
                item.CreatedAt = result.CreatedAt;

                _context.Entry(result).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }
    }
}
