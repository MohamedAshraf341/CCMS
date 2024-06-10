using CCMS.BE.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CCMS.BE.Data.Migrations
{
    public partial class InsertUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, Common.Const.Users.SuperAdmin.Password);

            migrationBuilder.InsertData(
               table: "Users",
               columns: new[] {
                   "Id",
                   "Name",
                   "Email",
                   "NormalizedEmail",
                   "EmailConfirmed",
                   "PasswordHash" ,
                   "SecurityStamp",
                   "AccessFailedCount",
                   "PhoneNumberConfirmed",
                   "TwoFactorEnabled",
                   "LockoutEnabled"
               },
               values: new object[] {
                   Common.Const.Users.SuperAdmin.Id,
                   Common.Const.Users.SuperAdmin.Name,
                   Common.Const.Users.SuperAdmin.Email,
                   Common.Const.Users.SuperAdmin.Email.ToUpper(),
                   Common.Const.Users.SuperAdmin.ConfirmEmail,
                   passwordHash,
                   Guid.NewGuid().ToString(),
                   0,
                   false,
                   false,
                   false
               },
               schema: "security"
           );
            migrationBuilder.Sql($@"
        INSERT INTO [security].[UserRoles] (UserId, RoleId) 
        VALUES ('{Common.Const.Users.SuperAdmin.Id}', '{Common.Const.Roles.SuperAdmin.Id}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM [security].[Users] WHERE Id = '{Common.Const.Users.SuperAdmin.Id}'");
            migrationBuilder.Sql($"DELETE FROM [security].[UserRoles] WHERE UserId = '{Common.Const.Users.SuperAdmin.Id}'");

        }
    }
}
