using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class Subadminupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ADMIN_DELEGATIONS_USER_UserId",
                table: "ADMIN_DELEGATIONS");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ADMIN_DELEGATIONS",
                newName: "SubAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_ADMIN_DELEGATIONS_UserId",
                table: "ADMIN_DELEGATIONS",
                newName: "IX_ADMIN_DELEGATIONS_SubAdminId");

            migrationBuilder.CreateTable(
                name: "SUB_ADMIN",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUB_ADMIN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SUB_ADMIN_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SUB_ADMIN_UserId",
                table: "SUB_ADMIN",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ADMIN_DELEGATIONS_SUB_ADMIN_SubAdminId",
                table: "ADMIN_DELEGATIONS",
                column: "SubAdminId",
                principalTable: "SUB_ADMIN",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ADMIN_DELEGATIONS_SUB_ADMIN_SubAdminId",
                table: "ADMIN_DELEGATIONS");

            migrationBuilder.DropTable(
                name: "SUB_ADMIN");

            migrationBuilder.RenameColumn(
                name: "SubAdminId",
                table: "ADMIN_DELEGATIONS",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ADMIN_DELEGATIONS_SubAdminId",
                table: "ADMIN_DELEGATIONS",
                newName: "IX_ADMIN_DELEGATIONS_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ADMIN_DELEGATIONS_USER_UserId",
                table: "ADMIN_DELEGATIONS",
                column: "UserId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
