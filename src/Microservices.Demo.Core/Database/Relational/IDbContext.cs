using Microservices.Demo.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Database.Relational
{
    public interface IDbContext
    {
        DbSet<ChangeLog> ChangeLogs { get; set; }

        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        Task<int> SaveChangesAsync(Guid userId);

        void Dispose();
    }
}
