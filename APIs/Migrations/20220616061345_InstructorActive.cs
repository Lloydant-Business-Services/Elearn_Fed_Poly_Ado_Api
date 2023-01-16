using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class InstructorActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "INSTRUCTOR_DEPARTMENT",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "INSTRUCTOR_DEPARTMENT");
        }
    }
}
