using Microservices.Demo.Core.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {
        Task<IQueryable<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(Guid id);

        Task<IQueryable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(IQueryable<TEntity> entities);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task<IQueryable<TEntity>> DeleteRangeAsync(IQueryable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<IQueryable<TEntity>> UpdateRangeAsync(IQueryable<TEntity> entities);
    }
}
