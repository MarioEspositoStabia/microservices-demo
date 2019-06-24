using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservices.Demo.IdentityService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Logging");

            migrationBuilder.EnsureSchema(
                name: "Authentication");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedBy = table.Column<Guid>(nullable: true),
                    UpdateNumber = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Hmac = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    LastActivityDate = table.Column<DateTimeOffset>(nullable: true),
                    Password = table.Column<string>(maxLength: 255, nullable: false),
                    Salt = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordQuestion = table.Column<string>(maxLength: 255, nullable: true),
                    PasswordAnswer = table.Column<string>(maxLength: 255, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    IsLockedOut = table.Column<bool>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastLockOutDate = table.Column<DateTimeOffset>(nullable: true),
                    FailedPasswordAttempt = table.Column<int>(nullable: true),
                    FailedPasswordAttemptDate = table.Column<DateTimeOffset>(nullable: true),
                    FailedPasswordAnswerAttempt = table.Column<int>(nullable: true),
                    FailedPasswordAnswerAttemptDate = table.Column<DateTimeOffset>(nullable: true),
                    Settings = table.Column<string>(maxLength: 4000, nullable: true),
                    VerificationCode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeLog",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityName = table.Column<string>(maxLength: 512, nullable: false),
                    Operation = table.Column<byte>(nullable: false),
                    PrimaryKey = table.Column<string>(maxLength: 512, nullable: false),
                    PropertyName = table.Column<string>(maxLength: 512, nullable: false),
                    OldValue = table.Column<string>(nullable: false),
                    NewValue = table.Column<string>(nullable: false),
                    ChangedBy = table.Column<string>(maxLength: 512, nullable: false),
                    ChangedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePhoto",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedBy = table.Column<Guid>(nullable: true),
                    UpdateNumber = table.Column<long>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Hmac = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Photo = table.Column<byte[]>(nullable: false),
                    FileExtension = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePhoto_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Authentication",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePhoto_UserId",
                schema: "Authentication",
                table: "ProfilePhoto",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Authentication",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                schema: "Authentication",
                table: "User",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePhoto",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "ChangeLog",
                schema: "Logging");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Authentication");
        }
    }
}
