using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDbContext _context;

        public Repository(IDbContext context)
        {
            this._context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await this._context.Set<TEntity>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public async Task AddRangeAsync(IQueryable<TEntity> entities)
        {
            await this._context.Set<TEntity>().AddRangeAsync(entities);
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> DeleteRangeAsync(IQueryable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> UpdateRangeAsync(IQueryable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
