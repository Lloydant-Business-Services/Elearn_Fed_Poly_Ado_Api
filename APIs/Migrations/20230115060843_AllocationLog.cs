using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class AllocationLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ALLOCATION_DEPARTMENT_LOG",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALLOCATION_DEPARTMENT_LOG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ALLOCATION_DEPARTMENT_LOG_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ALLOCATION_DEPARTMENT_LOG_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALLOCATION_DEPARTMENT_LOG_CourseAllocationId",
                table: "ALLOCATION_DEPARTMENT_LOG",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ALLOCATION_DEPARTMENT_LOG_DepartmentId",
                table: "ALLOCATION_DEPARTMENT_LOG",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALLOCATION_DEPARTMENT_LOG");
        }
    }
}
