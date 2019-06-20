using Microservices.Demo.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Demo.Core.Database.Relational.Mapping
{
    public class ChangeLogMapping : IEntityTypeConfiguration<ChangeLog>
    {
        public void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            builder.ToTable("ChangeLog", "Logging");

            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.EntityName).IsRequired().HasMaxLength(512);
            builder.Property(x => x.Operation).IsRequired();
            builder.Property(x => x.PrimaryKey).IsRequired().HasMaxLength(512);
            builder.Property(x => x.PropertyName).IsRequired().HasMaxLength(512);
            builder.Property(x => x.OldValue).IsRequired();
            builder.Property(x => x.NewValue).IsRequired();
            builder.Property(x => x.ChangedBy).IsRequired().HasMaxLength(512);
            builder.Property(x => x.ChangedDate).IsRequired();
        }
    }
}
