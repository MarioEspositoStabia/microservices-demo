using Microservices.Demo.Core.Database.Relational.Mapping.EntityTypeConfigurations;
using Microservices.Demo.IdentityService.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Demo.IdentityService.Database.Mapping
{
    public class ProfilePhotoMapping : AuditableEntityTypeConfiguration<ProfilePhoto>
    {
        public ProfilePhotoMapping()
           : base("ProfilePhoto", false, "Authentication")
        {
        }

        public override void Configure(EntityTypeBuilder<ProfilePhoto> builder)
        {
            base.Configure(builder);

            builder.Property(profilePhoto => profilePhoto.Photo).IsRequired();
            builder.Property(profilePhoto => profilePhoto.FileExtension).IsRequired();

            builder.HasOne(profilePhoto => profilePhoto.User)
                   .WithOne(user => user.ProfilePhoto)
                   .HasForeignKey<ProfilePhoto>(profilePhoto => profilePhoto.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
