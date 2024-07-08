using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCMS.BE.Data.Migrations
{
    public partial class AddBranchUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchUser_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchUser_Users_UserId1",
                        column: x => x.UserId1,
                        principalSchema: "security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchUser_BranchId",
                table: "BranchUser",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchUser_UserId1",
                table: "BranchUser",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchUser");
        }
    }
}
