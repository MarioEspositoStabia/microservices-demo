using Microservices.Demo.Core.Database.Relational.Mapping.EntityTypeConfigurations;
using Microservices.Demo.IdentityService.Database.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Demo.IdentityService.Database.Mapping
{
    public class UserMapping : AuditableEntityTypeConfiguration<User>
    {
        public UserMapping()
            : base("User", false, "Authentication")
        {
        }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(user => user.UserName).HasMaxLength(50);
            builder.HasIndex(user => user.UserName)
                   .IsUnique();

            builder.Property(user => user.LastActivityDate);

            builder.Property(user => user.Password).IsRequired().HasMaxLength(255);

            builder.Property(user => user.Salt).IsRequired().HasMaxLength(255);

            builder.Property(user => user.PasswordQuestion).HasMaxLength(255);

            builder.Property(user => user.PasswordAnswer).HasMaxLength(255);

            builder.Property(user => user.Email).HasMaxLength(50);
            builder.HasIndex(user => user.Email)
                   .IsUnique();

            builder.Property(user => user.EmailConfirmed).IsRequired();

            builder.Property(user => user.IsLockedOut).IsRequired();

            builder.Property(user => user.LastPasswordChangedDate);

            builder.Property(user => user.LastLockOutDate);

            builder.Property(user => user.FailedPasswordAttempt);

            builder.Property(user => user.FailedPasswordAttemptDate);

            builder.Property(user => user.FailedPasswordAnswerAttempt);

            builder.Property(user => user.FailedPasswordAnswerAttemptDate);

            builder.Property(user => user.Settings).HasMaxLength(4000);

            builder.Property(user => user.VerificationCode).HasMaxLength(50);
        }
    }
}
