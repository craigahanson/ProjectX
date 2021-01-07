using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Data
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(long id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        Task RemoveWhereAsync(Expression<Func<T, bool>> where);
        void RemoveRange(IEnumerable<T> entities);
        IQueryable<T> GetQueryable();
        Task<List<Tin>> QueryableToListAsync<Tin>(IQueryable<Tin> queryable);
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> where);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where);
        Task<List<Tout>> ToListAsync<Tout>(Func<IQueryable<T>, IQueryable<Tout>> query);
    }
}
