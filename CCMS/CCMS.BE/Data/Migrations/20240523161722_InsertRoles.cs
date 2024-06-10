using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CCMS.BE.Data.Migrations
{
    public partial class InsertRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Roles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Common.Const.Roles.User.Id, Common.Const.Roles.User.Name, Common.Const.Roles.User.Name.ToUpper(), Guid.NewGuid().ToString() },
               schema: "security"
           );

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Common.Const.Roles.Admin.Id, Common.Const.Roles.Admin.Name, Common.Const.Roles.Admin.Name.ToUpper(), Guid.NewGuid().ToString() },
                schema: "security"
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Common.Const.Roles.SuperAdmin.Id, Common.Const.Roles.SuperAdmin.Name, Common.Const.Roles.SuperAdmin.Name.ToUpper(), Guid.NewGuid().ToString() },
                schema: "security"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Roles]");

        }
    }
}
