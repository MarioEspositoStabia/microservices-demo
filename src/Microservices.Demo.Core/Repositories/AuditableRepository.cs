using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Entity;
using Microservices.Demo.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories
{
    public class AuditableRepository<TEntity> : IAuditableRepository<TEntity> where TEntity : AuditableEntity
    {
        private readonly IDbContext _context;

        public AuditableRepository(IDbContext context)
        {
            this._context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            this.LockAuditProperties(entity);
            entity.Status = EntityStatus.Available;
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

        public TEntity Delete(TEntity entity, bool isHardDelete = false)
        {
            entity.Status = isHardDelete ? EntityStatus.HardDeleted : EntityStatus.SoftDeleted;
            if (isHardDelete)
            {
                return HardDelete(entity);
            }
            else
            {
                this.LockAuditProperties(entity);
                return this.Update(entity);
            }
        }

        public IEnumerable<TEntity> DeleteRange(IQueryable<TEntity> entities, bool isHardDelete = false)
        {
            return isHardDelete ? DoForEach(entities, this.HardDelete, entity => entity.Status = EntityStatus.HardDeleted) :
                                  DoForEach(entities, this.Update, entity =>
                                  {
                                      entity.Status = EntityStatus.SoftDeleted;
                                      this.LockAuditProperties(entity);
                                  });
        }

        public async Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available)
        {
            return await this._context.Set<TEntity>().Where(predicate).Where(entity => entity.Status == entityStatus).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(EntityStatus entityStatus = EntityStatus.Available)
        {
            return await this._context.Set<TEntity>().Where(entity => entity.Status == entityStatus).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Guid id, EntityStatus entityStatus = EntityStatus.Available)
        {
            return await this._context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id.Equals(id) && entity.Status == entityStatus);
        }

        public TEntity Update(TEntity entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            this.LockAuditProperties(entity);
            return entity;
        }

        public IEnumerable<TEntity> UpdateRange(IQueryable<TEntity> entities)
        {
            return DoForEach(entities, this.Update);
        }

        private TEntity HardDelete(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = this._context.Set<TEntity>().Remove(entity);
            return entityEntry.Entity;
        }

        private void LockAuditProperties(TEntity entity)
        {
            if (entity.RowVersion == null)
            {
                this._context.Entry(entity).Property(p => p.LastModifiedBy).IsModified = false;
                this._context.Entry(entity).Property(p => p.LastModifiedDate).IsModified = false;
                this._context.Entry(entity).Property(p => p.UpdateNumber).IsModified = false;
            }
            else
            {
                this._context.Entry(entity).Property(p => p.CreatedDate).IsModified = false;
                this._context.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            }
        }

        private TEntity[] DoForEach(IQueryable<TEntity> entities, Func<TEntity, TEntity> function, Action<TEntity> action = null)
        {
            TEntity[] enumerable = entities as TEntity[] ?? entities.ToArray();
            TEntity[] entityList = new TEntity[enumerable.Length];

            for (int i = 0; i < enumerable.Length; i++)
            {
                TEntity entity = enumerable[i];

                if (action != null)
                {
                    action.Invoke(entity);
                }

                entityList[i] = function.Invoke(entity);
            }

            return entityList;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available)
        {
            return await this._context.Set<TEntity>().Where(entity => entity.Status == entityStatus).AnyAsync(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, EntityStatus entityStatus = EntityStatus.Available)
        {
            return await this._context.Set<TEntity>().Where(entity => entity.Status == entityStatus).SingleOrDefaultAsync(predicate);
        }
    }
}
