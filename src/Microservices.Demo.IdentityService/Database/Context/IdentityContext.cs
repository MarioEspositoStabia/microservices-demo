using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Enumerations;
using Microservices.Demo.IdentityService.Database.Entity;
using Microservices.Demo.IdentityService.Database.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Demo.IdentityService.Database.Context
{
    public class IdentityContext : BaseContext
    {
        public IdentityContext(DatabaseProvider databaseProvider, string connectionString, bool logAuditableTables = false)
           : base(databaseProvider, connectionString, logAuditableTables)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ProfilePhoto> ProfilePhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserMapping());
            builder.ApplyConfiguration(new ProfilePhotoMapping());
        }
    }
}
