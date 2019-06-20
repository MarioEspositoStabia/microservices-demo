using Microservices.Demo.Core.Entity;
using Microservices.Demo.Core.Enumerations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories
{
    public interface IAuditableRepository<TEntity> where TEntity : IAuditableEntity
    {
        Task<IQueryable<TEntity>> GetAllAsync(EntityStatus entityStatus = EntityStatus.Available);

        Task<TEntity> GetAsync(Guid id, EntityStatus entityStatus = EntityStatus.Available);

        Task<IQueryable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available);

        Task<TEntity> AddAsync(TEntity entity);

        Task<IQueryable<TEntity>> AddRangeAsync(IQueryable<TEntity> entities);

        Task<TEntity> DeleteAsync(TEntity entity, bool isHardDelete = false);

        Task<IQueryable<TEntity>> DeleteRangeAsync(IQueryable<TEntity> entities, bool isHardDelete = false);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<IQueryable<TEntity>> UpdateRangeAsync(IQueryable<TEntity> entities);
    }
}
