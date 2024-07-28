using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCMS.BE.Data.Migrations
{
    public partial class UpdateBranchUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchUser_Users_UserId1",
                table: "BranchUser");

            migrationBuilder.DropIndex(
                name: "IX_BranchUser_UserId1",
                table: "BranchUser");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BranchUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BranchUser",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_BranchUser_UserId",
                table: "BranchUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchUser_Users_UserId",
                table: "BranchUser",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchUser_Users_UserId",
                table: "BranchUser");

            migrationBuilder.DropIndex(
                name: "IX_BranchUser_UserId",
                table: "BranchUser");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BranchUser",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "BranchUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BranchUser_UserId1",
                table: "BranchUser",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchUser_Users_UserId1",
                table: "BranchUser",
                column: "UserId1",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
