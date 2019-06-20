using Microservices.Demo.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Demo.Core.Database.Relational.Mapping.EntityTypeConfigurations
{
    public abstract class BaseEntityTypeConfiguration<TEntity> : EntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        private readonly bool _isIdentity;

        public BaseEntityTypeConfiguration(string tableName, bool isIdentity, string schemaName = "")
            : base(tableName, schemaName)
        {
            this._isIdentity = isIdentity;
        }

        protected bool IsIdentity
        {
            get
            {
                return this._isIdentity;
            }
        }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);

            if (this._isIdentity)
            {
                builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(entity => entity.Id).ValueGeneratedNever();
            }

            base.Configure(builder);
        }
    }
}
