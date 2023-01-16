using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class crossUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STUDENT_PAYMENT_STUDENT_PERSON_StudentPersonId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.RenameColumn(
                name: "StudentPersonId",
                table: "STUDENT_PAYMENT",
                newName: "SessionSemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_STUDENT_PAYMENT_StudentPersonId",
                table: "STUDENT_PAYMENT",
                newName: "IX_STUDENT_PAYMENT_SessionSemesterId");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "STUDENT_PAYMENT",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentSetupId",
                table: "STUDENT_PAYMENT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "STUDENT_PAYMENT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PAYMENT_SETUP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENT_SETUP", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_PaymentSetupId",
                table: "STUDENT_PAYMENT",
                column: "PaymentSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_PersonId",
                table: "STUDENT_PAYMENT",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_STUDENT_PAYMENT_PAYMENT_SETUP_PaymentSetupId",
                table: "STUDENT_PAYMENT",
                column: "PaymentSetupId",
                principalTable: "PAYMENT_SETUP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_STUDENT_PAYMENT_PERSON_PersonId",
                table: "STUDENT_PAYMENT",
                column: "PersonId",
                principalTable: "PERSON",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_STUDENT_PAYMENT_SESSION_SEMESTER_SessionSemesterId",
                table: "STUDENT_PAYMENT",
                column: "SessionSemesterId",
                principalTable: "SESSION_SEMESTER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STUDENT_PAYMENT_PAYMENT_SETUP_PaymentSetupId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_STUDENT_PAYMENT_PERSON_PersonId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_STUDENT_PAYMENT_SESSION_SEMESTER_SessionSemesterId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropTable(
                name: "PAYMENT_SETUP");

            migrationBuilder.DropIndex(
                name: "IX_STUDENT_PAYMENT_PaymentSetupId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropIndex(
                name: "IX_STUDENT_PAYMENT_PersonId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropColumn(
                name: "PaymentSetupId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "STUDENT_PAYMENT");

            migrationBuilder.RenameColumn(
                name: "SessionSemesterId",
                table: "STUDENT_PAYMENT",
                newName: "StudentPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_STUDENT_PAYMENT_SessionSemesterId",
                table: "STUDENT_PAYMENT",
                newName: "IX_STUDENT_PAYMENT_StudentPersonId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "STUDENT_PAYMENT",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_STUDENT_PAYMENT_STUDENT_PERSON_StudentPersonId",
                table: "STUDENT_PAYMENT",
                column: "StudentPersonId",
                principalTable: "STUDENT_PERSON",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
