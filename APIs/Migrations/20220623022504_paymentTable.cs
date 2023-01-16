using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class paymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STUDENT_PAYMENT_LEVEL_LevelId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropIndex(
                name: "IX_STUDENT_PAYMENT_LevelId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropColumn(
                name: "GatewayCode",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "STUDENT_PAYMENT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "STUDENT_PAYMENT",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "GatewayCode",
                table: "STUDENT_PAYMENT",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LevelId",
                table: "STUDENT_PAYMENT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_LevelId",
                table: "STUDENT_PAYMENT",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_STUDENT_PAYMENT_LEVEL_LevelId",
                table: "STUDENT_PAYMENT",
                column: "LevelId",
                principalTable: "LEVEL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
