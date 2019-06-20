using Microservices.Demo.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Demo.Core.Database.Relational.Mapping.EntityTypeConfigurations
{
    public abstract class AuditableEntityTypeConfiguration<TEntity> : EntityTypeConfiguration<TEntity>
       where TEntity : AuditableEntity
    {
        private readonly bool _isIdentity;

        public AuditableEntityTypeConfiguration(string tableName, bool isIdentity, string schemaName = "")
            : base(tableName, schemaName)
        {
            this._isIdentity = isIdentity;
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

            builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
            builder.Property(entity => entity.CreatedBy).IsRequired();
            builder.Property(entity => entity.CreatedDate).IsRequired();
            builder.Property(entity => entity.LastModifiedBy);
            builder.Property(entity => entity.LastModifiedDate);
            builder.Property(entity => entity.UpdateNumber);
            builder.Property(entity => entity.Status).IsRequired();
            builder.Property(entity => entity.RowVersion).IsRowVersion();
            builder.Property(entity => entity.Hmac).IsRequired();

            base.Configure(builder);
        }
    }
}
