using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class passwordUpdade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordUpdated",
                table: "USER",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasswordUpdated",
                table: "USER");
        }
    }
}
