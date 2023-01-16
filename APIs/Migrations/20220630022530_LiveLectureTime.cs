using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class LiveLectureTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "CLASS_MEETINGS",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "CLASS_MEETINGS");
        }
    }
}
