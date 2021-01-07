using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectX.Data.Scope;

namespace ProjectX.Data.EntityFrameworkCore
{
    public class RepositoryBase
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;

        public RepositoryBase(IAmbientDbContextLocator ambientDbContextLocator)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        protected ProjectXDbContext DbContext
        {
            get
            {
                var dbContext = ambientDbContextLocator.Get<ProjectXDbContext>();
                if (dbContext == null) throw new ArgumentNullException("DbContext is null.  Have you forgotton to wrap your data access code in a scope, like 'using (var scope = dbContextScopeFactory.Create()){' etc.");
                return dbContext;
            }
        }
    }

    public class RepositoryBase<T> : RepositoryBase, IRepository<T> where T : class
    {
        public RepositoryBase(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }

        protected DbSet<T> DbSet => DbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            return (await DbSet.AddAsync(entity)).Entity;
        }

        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            return DbSet.AddRangeAsync(entities);
        }

        public Task<List<T>> GetAllAsync()
        {
            return DbSet.ToListAsync();
        }

        public Task<T> GetAsync(long id)
        {
            return DbSet.FindAsync(id).AsTask();
        }

        public IQueryable<T> GetQueryable()
        {
            return DbSet;
        }

        public Task<List<Tin>> QueryableToListAsync<Tin>(IQueryable<Tin> queryable)
        {
            return queryable.ToListAsync();
        }

        public async Task RemoveWhereAsync(Expression<Func<T, bool>> where)
        {
            RemoveRange(await GetWhereAsync(where));
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).ToListAsync();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return DbSet.FirstOrDefaultAsync(where);
        }

        public Task<List<Tout>> ToListAsync<Tout>(Func<IQueryable<T>, IQueryable<Tout>> query)
        {
            return query(DbSet).ToListAsync();
        }

        public Task<T> GetAsync(int id)
        {
            return DbSet.FindAsync(id).AsTask();
        }
    }
}