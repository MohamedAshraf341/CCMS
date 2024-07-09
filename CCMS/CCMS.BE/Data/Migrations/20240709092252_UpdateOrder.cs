using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCMS.BE.Data.Migrations
{
    public partial class UpdateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedBy",
                table: "Orders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceivedBy",
                table: "Orders",
                column: "ReceivedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CreatedBy",
                table: "Orders",
                column: "CreatedBy",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_ReceivedBy",
                table: "Orders",
                column: "ReceivedBy",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CreatedBy",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_ReceivedBy",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreatedBy",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReceivedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                table: "Orders");
        }
    }
}
