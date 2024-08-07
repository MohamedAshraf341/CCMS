﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCMS.BE.Data.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemType",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemType",
                schema: "security",
                table: "Users");
        }
    }
}
