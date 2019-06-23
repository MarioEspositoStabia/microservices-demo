using Microservices.Demo.Core.Entity;
using Microservices.Demo.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories
{
    public interface IAuditableRepository<TEntity> where TEntity : IAuditableEntity
    {
        Task<List<TEntity>> GetAllAsync(EntityStatus entityStatus = EntityStatus.Available);

        Task<TEntity> GetAsync(Guid id, EntityStatus entityStatus = EntityStatus.Available);

        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available);

        Task<TEntity> AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> AddRangeAsync(IQueryable<TEntity> entities);

        TEntity Delete(TEntity entity, bool isHardDelete = false);

        IEnumerable<TEntity> DeleteRange(IQueryable<TEntity> entities, bool isHardDelete = false);

        TEntity Update(TEntity entity);

        IEnumerable<TEntity> UpdateRange(IQueryable<TEntity> entities);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available);
    }
}
