using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class characterLimit_Assignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MaxCharacters",
                table: "ASSIGNMENT",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxCharacters",
                table: "ASSIGNMENT");
        }
    }
}
