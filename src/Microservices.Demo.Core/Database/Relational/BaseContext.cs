using Microservices.Demo.Core.Database.Relational.Mapping;
using Microservices.Demo.Core.Entity;
using Microservices.Demo.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Database.Relational
{
    public class BaseContext : DbContext, IDbContext
    {
        private const string _migrationTableSchema = "Migration";

        private readonly string _connectionString;

        private readonly DatabaseProvider _databaseProvider;

        private readonly bool _logAuditableTables;

        private bool _isDisposed;

        private bool _isLocalCall = false;

        public BaseContext(DatabaseProvider databaseProvider, string connectionString, bool logAuditableTables = false)
        {
            this._connectionString = connectionString;
            this._databaseProvider = databaseProvider;
            this._logAuditableTables = logAuditableTables;
        }

        ~BaseContext()
        {
            this.Dispose(false);
        }

        public DbSet<ChangeLog> ChangeLogs { get; set; }

        public Task<int> SaveChangesAsync(Guid userId)
        {
            Task<int> resultCount;
            this._isLocalCall = true;
            if (userId == null)
            {
                resultCount = base.SaveChangesAsync();
                this._isLocalCall = false;
                return resultCount;
            }

            this.TrackAuditableChanges(userId);

            resultCount = base.SaveChangesAsync();
            this._isLocalCall = false;
            return resultCount;
        }

        public int SaveChanges(Guid userId)
        {
            int resultCount;
            this._isLocalCall = true;
            if (userId == null)
            {
                resultCount = base.SaveChanges();
                this._isLocalCall = false;
                return resultCount;
            }

            this.TrackAuditableChanges(userId);

            resultCount = base.SaveChanges();
            this._isLocalCall = false;
            return resultCount;
        }

        public override int SaveChanges()
        {
            if (!this._isLocalCall)
            {
                throw new NotSupportedException();
            }

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (!this._isLocalCall)
            {
                throw new NotSupportedException();
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!this._isLocalCall)
            {
                throw new NotSupportedException();
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!this._isLocalCall)
            {
                throw new NotSupportedException();
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override void Dispose()
        {
            this.Dispose(true);
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed && disposing)
            {
                this.DisposeCore();
            }

            this._isDisposed = true;
        }

        protected virtual void DisposeCore()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ChangeLogMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                switch (this._databaseProvider)
                {
                    case DatabaseProvider.MicrosoftSQLServer:
                        optionsBuilder.UseSqlServer(this._connectionString, options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, _migrationTableSchema));
                        break;
                    case DatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(this._connectionString, options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, _migrationTableSchema));
                        break;
                    case DatabaseProvider.Sqlite:
                        optionsBuilder.UseSqlite(this._connectionString, options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, _migrationTableSchema));
                        break;
                    case DatabaseProvider.InMemory:
                        optionsBuilder.UseInMemoryDatabase($"{this.GetType().Name}-TestDatabase");
                        break;
                    case DatabaseProvider.PostgreSQL:
                        optionsBuilder.UseNpgsql(this._connectionString, options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, _migrationTableSchema));
                        break;
                    case DatabaseProvider.Oracle:
                        optionsBuilder.UseOracle(this._connectionString, options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, _migrationTableSchema));
                        break;
                    default:
                        break;
                }
            }
        }

        private string GetPrimaryKeyValue(EntityEntry entry)
        {
            Type entityType = entry.Entity.GetType();
            List<PropertyInfo> primaryKeyProperties = this.Model.FindEntityType(entityType).FindPrimaryKey().Properties.Select(x => x.PropertyInfo).ToList();
            return string.Join(",", primaryKeyProperties.Select(x => x.GetValue(entry.Entity).ToString()));
        }

        private void TrackAuditableChanges(Guid userId)
        {
            List<EntityEntry<IAuditableEntity>> entries = this.ChangeTracker.Entries<IAuditableEntity>()
                                                                            .Where(entityEntry => entityEntry.State == EntityState.Modified ||
                                                                                                  entityEntry.State == EntityState.Deleted ||
                                                                                                  entityEntry.State == EntityState.Added).ToList();
            DateTimeOffset now = DateTimeOffset.UtcNow;

            foreach (EntityEntry<IAuditableEntity> entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedDate = now;
                    ++entry.Entity.UpdateNumber;
                }

                if (!this._logAuditableTables)
                {
                    continue;
                }

                Type type = entry.Entity.GetType();

                string entityName = type.Name;
                string primaryKey = this.GetPrimaryKeyValue(entry);

                if (entry.State == EntityState.Added)
                {
                    foreach (string propertyName in entry.CurrentValues.Properties.Select(x => x.Name))
                    {
                        object propertyValues = entry.CurrentValues[propertyName];
                        string currentValue = propertyValues == null ? "NULL" : propertyValues.ToString();
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            Operation = EntityOperation.Added,
                            PrimaryKey = primaryKey,
                            PropertyName = propertyName,
                            NewValue = currentValue,
                            OldValue = string.Empty,
                            ChangedBy = userId.ToString(),
                            ChangedDate = now
                        };
                        this.ChangeLogs.Add(log);
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    foreach (string propertyName in entry.OriginalValues.Properties.Select(x => x.Name))
                    {
                        object propertyValues = entry.CurrentValues[propertyName];
                        string currentValue = propertyValues == null ? "NULL" : propertyValues.ToString();
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            Operation = EntityOperation.Deleted,
                            PrimaryKey = primaryKey,
                            PropertyName = propertyName,
                            NewValue = string.Empty,
                            OldValue = currentValue,
                            ChangedBy = userId.ToString(),
                            ChangedDate = now
                        };
                        this.ChangeLogs.Add(log);
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    foreach (string propertyName in entry.OriginalValues.Properties.Select(x => x.Name))
                    {
                        object originalPropertyValues = entry.OriginalValues[propertyName];
                        string originalValue = originalPropertyValues == null ? "NULL" : originalPropertyValues.ToString();
                        
                        object propertyValues = entry.CurrentValues[propertyName];
                        string currentValue = propertyValues == null ? "NULL" : propertyValues.ToString();

                        if (originalValue != currentValue)
                        {
                            ChangeLog log = new ChangeLog()
                            {
                                EntityName = entityName,
                                PrimaryKey = primaryKey,
                                Operation = EntityOperation.Modified,
                                PropertyName = propertyName,
                                OldValue = originalValue,
                                NewValue = currentValue,
                                ChangedBy = userId.ToString(),
                                ChangedDate = now
                            };
                            this.ChangeLogs.Add(log);
                        }
                    }
                }
            }
        }
    }
}
