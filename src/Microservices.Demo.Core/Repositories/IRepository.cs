using Microservices.Demo.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(Guid id);

        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task<IEnumerable<TEntity>> AddRangeAsync(IQueryable<TEntity> entities);

        TEntity Delete(TEntity entity);

        IEnumerable<TEntity> DeleteRange(IQueryable<TEntity> entities);

        TEntity Update(TEntity entity);

        IEnumerable<TEntity> UpdateRange(IQueryable<TEntity> entities);
    }
}
