﻿// <auto-generated />
using System;
using Microservices.Demo.IdentityService.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microservices.Demo.IdentityService.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microservices.Demo.Core.Entity.ChangeLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<DateTimeOffset>("ChangedDate");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("NewValue")
                        .IsRequired();

                    b.Property<string>("OldValue")
                        .IsRequired();

                    b.Property<byte>("Operation");

                    b.Property<string>("PrimaryKey")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.ToTable("ChangeLog","Logging");
                });

            modelBuilder.Entity("Microservices.Demo.IdentityService.Database.Entity.ProfilePhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("FileExtension")
                        .IsRequired();

                    b.Property<string>("Hmac")
                        .IsRequired();

                    b.Property<Guid?>("LastModifiedBy");

                    b.Property<DateTimeOffset?>("LastModifiedDate");

                    b.Property<byte[]>("Photo")
                        .IsRequired();

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<short>("Status");

                    b.Property<long>("UpdateNumber");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ProfilePhoto","Authentication");
                });

            modelBuilder.Entity("Microservices.Demo.IdentityService.Database.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("FailedPasswordAnswerAttempt");

                    b.Property<DateTimeOffset?>("FailedPasswordAnswerAttemptDate");

                    b.Property<int?>("FailedPasswordAttempt");

                    b.Property<DateTimeOffset?>("FailedPasswordAttemptDate");

                    b.Property<string>("Hmac")
                        .IsRequired();

                    b.Property<bool>("IsLockedOut");

                    b.Property<DateTimeOffset?>("LastActivityDate");

                    b.Property<DateTimeOffset?>("LastLockOutDate");

                    b.Property<Guid?>("LastModifiedBy");

                    b.Property<DateTimeOffset?>("LastModifiedDate");

                    b.Property<DateTimeOffset?>("LastPasswordChangedDate");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("PasswordAnswer")
                        .HasMaxLength(255);

                    b.Property<string>("PasswordQuestion")
                        .HasMaxLength(255);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Settings")
                        .HasMaxLength(4000);

                    b.Property<short>("Status");

                    b.Property<long>("UpdateNumber");

                    b.Property<string>("UserName")
                        .HasMaxLength(50);

                    b.Property<string>("VerificationCode")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("User","Authentication");
                });

            modelBuilder.Entity("Microservices.Demo.IdentityService.Database.Entity.ProfilePhoto", b =>
                {
                    b.HasOne("Microservices.Demo.IdentityService.Database.Entity.User", "User")
                        .WithOne("ProfilePhoto")
                        .HasForeignKey("Microservices.Demo.IdentityService.Database.Entity.ProfilePhoto", "UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
