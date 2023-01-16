using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class Quiz_Email_Tracker_MG_21_06_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLateSubmission",
                table: "ASSIGNMENT_SUBMISSION",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CharacterLimit",
                table: "ASSIGNMENT",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QUIZ",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizInText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizVideoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizUploadLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PublishResult = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterLimit = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUIZ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QUIZ_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT_PAYMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    SessionId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StatusCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientPortalIdentifier = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    ProgrammeId = table.Column<long>(type: "bigint", nullable: true),
                    GatewayCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SystemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SystemPaymentReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentGateway = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPersonId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMode = table.Column<int>(type: "int", nullable: true),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT_PAYMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STUDENT_PAYMENT_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_PAYMENT_LEVEL_LevelId",
                        column: x => x.LevelId,
                        principalTable: "LEVEL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_PAYMENT_SESSION_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SESSION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_PAYMENT_STUDENT_PERSON_StudentPersonId",
                        column: x => x.StudentPersonId,
                        principalTable: "STUDENT_PERSON",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QUIZ_SUBMISSION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizInTextSubmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizSubmissionUploadLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizSubmissionHostedLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructorRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsGraded = table.Column<bool>(type: "bit", nullable: true),
                    IsLateSubmission = table.Column<bool>(type: "bit", nullable: true),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    QuizId = table.Column<long>(type: "bigint", nullable: false),
                    CourseRegistrationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUIZ_SUBMISSION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QUIZ_SUBMISSION_COURSE_REGISTRATION_CourseRegistrationId",
                        column: x => x.CourseRegistrationId,
                        principalTable: "COURSE_REGISTRATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QUIZ_SUBMISSION_QUIZ_QuizId",
                        column: x => x.QuizId,
                        principalTable: "QUIZ",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QUIZ_CourseAllocationId",
                table: "QUIZ",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_QUIZ_SUBMISSION_CourseRegistrationId",
                table: "QUIZ_SUBMISSION",
                column: "CourseRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_QUIZ_SUBMISSION_QuizId",
                table: "QUIZ_SUBMISSION",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_DepartmentId",
                table: "STUDENT_PAYMENT",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_LevelId",
                table: "STUDENT_PAYMENT",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_SessionId",
                table: "STUDENT_PAYMENT",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PAYMENT_StudentPersonId",
                table: "STUDENT_PAYMENT",
                column: "StudentPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QUIZ_SUBMISSION");

            migrationBuilder.DropTable(
                name: "STUDENT_PAYMENT");

            migrationBuilder.DropTable(
                name: "QUIZ");

            migrationBuilder.DropColumn(
                name: "IsLateSubmission",
                table: "ASSIGNMENT_SUBMISSION");

            migrationBuilder.DropColumn(
                name: "CharacterLimit",
                table: "ASSIGNMENT");
        }
    }
}
