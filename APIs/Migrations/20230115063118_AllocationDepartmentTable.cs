using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class AllocationDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "COURSE_ALLOCATION",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_DepartmentId",
                table: "COURSE_ALLOCATION",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_COURSE_ALLOCATION_DEPARTMENT_DepartmentId",
                table: "COURSE_ALLOCATION",
                column: "DepartmentId",
                principalTable: "DEPARTMENT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_COURSE_ALLOCATION_DEPARTMENT_DepartmentId",
                table: "COURSE_ALLOCATION");

            migrationBuilder.DropIndex(
                name: "IX_COURSE_ALLOCATION_DepartmentId",
                table: "COURSE_ALLOCATION");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "COURSE_ALLOCATION");
        }
    }
}
