using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IQueryable<TEntity> entities)
        {
            TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
            TEntity[] entityList = new TEntity[enumerable.Length];

            for (int i = 0; i < enumerable.Length; i++)
            {
                TEntity entity = enumerable[i];
                entityList[i] = await this.AddAsync(entity);
            }

            return entityList;
        }

        public TEntity Delete(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = this._context.Set<TEntity>().Remove(entity);
            return entityEntry.Entity;
        }

        public IEnumerable<TEntity> DeleteRange(IQueryable<TEntity> entities)
        {
            return DoForEach(entities, this.Delete);
        }

        public async Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this._context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await this._context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await this._context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public TEntity Update(TEntity entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IEnumerable<TEntity> UpdateRange(IQueryable<TEntity> entities)
        {
            return DoForEach(entities, this.Update);
        }

        private TEntity[] DoForEach(IQueryable<TEntity> entities, Func<TEntity, TEntity> function)
        {
            TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
            TEntity[] entityList = new TEntity[enumerable.Length];

            for (int i = 0; i < enumerable.Length; i++)
            {
                TEntity entity = enumerable[i];

                entityList[i] = function.Invoke(entity);
            }

            return entityList;
        }
    }
}
