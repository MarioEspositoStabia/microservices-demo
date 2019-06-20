using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Microservices.Demo.Core.Database.Relational.Mapping.EntityTypeConfigurations
{
    public class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        private readonly string _tableName;

        private readonly string _schemaName;

        public EntityTypeConfiguration(string tableName, string schemaName = "")
        {
            if (tableName.Length > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(tableName));
            }

            this._tableName = tableName;
            this._schemaName = schemaName;
        }

        protected string TableName
        {
            get
            {
                return this._tableName;
            }
        }

        protected string SchemaName
        {
            get
            {
                return this._schemaName;
            }
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (string.IsNullOrWhiteSpace(this._schemaName))
            {
                builder.ToTable(this._tableName);
            }
            else
            {
                builder.ToTable(this._tableName, this._schemaName);
            }
        }
    }
}
