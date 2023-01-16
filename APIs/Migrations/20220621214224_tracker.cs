using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class tracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TItle",
                table: "NOTIFICATION_TRACKER",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TItle",
                table: "NOTIFICATION_TRACKER");
        }
    }
}
