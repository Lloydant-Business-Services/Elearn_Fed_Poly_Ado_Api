using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIs.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ANSWER_OPTIONS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANSWER_OPTIONS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FACULTY_SCHOOL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FACULTY_SCHOOL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GENDER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GENDER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "INSTITUTION_TYPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INSTITUTION_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LEVEL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEVEL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PERSON_TYPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON_TYPE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROLE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SECURITY_QUESTION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SECURITY_QUESTION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEMESTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEMESTER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SESSION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DEPARTMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FacultySchoolId = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_FACULTY_SCHOOL_FacultySchoolId",
                        column: x => x.FacultySchoolId,
                        principalTable: "FACULTY_SCHOOL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PERSON",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Othername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    GenderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSON", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PERSON_GENDER_GenderId",
                        column: x => x.GenderId,
                        principalTable: "GENDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SESSION_SEMESTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<long>(type: "bigint", nullable: false),
                    SemesterId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSION_SEMESTER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SESSION_SEMESTER_SEMESTER_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "SEMESTER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SESSION_SEMESTER_SESSION_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SESSION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT_PERSON",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatricNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MatricNoSlug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT_PERSON", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STUDENT_PERSON_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_PERSON_PERSON_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PERSON",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignUpDate = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USER_PERSON_PersonId",
                        column: x => x.PersonId,
                        principalTable: "PERSON",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USER_ROLE_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ROLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ANNOUNCEMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANNOUNCEMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ANNOUNCEMENT_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ANNOUNCEMENT_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COURSE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CourseCodeSlug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CourseTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CourseTitleSlug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LevelId = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COURSE_LEVEL_LevelId",
                        column: x => x.LevelId,
                        principalTable: "LEVEL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DEPARTMENT_HEADS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENT_HEADS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_HEADS_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DEPARTMENT_HEADS_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GENERAL_AUDIT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ActionPerformed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Client = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<long>(type: "bigint", nullable: false),
                    InitialValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GENERAL_AUDIT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GENERAL_AUDIT_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OTP_CODE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    OTPStatus = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTP_CODE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OTP_CODE_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COURSE_ALLOCATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    InstructorId = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionSemesterId = table.Column<long>(type: "bigint", nullable: false),
                    LevelId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE_ALLOCATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COURSE_ALLOCATION_COURSE_CourseId",
                        column: x => x.CourseId,
                        principalTable: "COURSE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_ALLOCATION_LEVEL_LevelId",
                        column: x => x.LevelId,
                        principalTable: "LEVEL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_ALLOCATION_SESSION_SEMESTER_SessionSemesterId",
                        column: x => x.SessionSemesterId,
                        principalTable: "SESSION_SEMESTER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_ALLOCATION_USER_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_ALLOCATION_USER_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ASSIGNMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentInText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentVideoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentUploadLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PublishResult = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASSIGNMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASSIGNMENT_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CLASS_MEETINGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Agenda = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Duration = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Start_Meeting_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Join_Meeting_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLASS_MEETINGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CLASS_MEETINGS_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CLASS_MEETINGS_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COURSE_REGISTRATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentPersonId = table.Column<long>(type: "bigint", nullable: false),
                    SessionSemesterId = table.Column<long>(type: "bigint", nullable: false),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE_REGISTRATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COURSE_REGISTRATION_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_REGISTRATION_SESSION_SEMESTER_SessionSemesterId",
                        column: x => x.SessionSemesterId,
                        principalTable: "SESSION_SEMESTER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_REGISTRATION_STUDENT_PERSON_StudentPersonId",
                        column: x => x.StudentPersonId,
                        principalTable: "STUDENT_PERSON",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COURSE_SUB_ALLOCATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    SubInstructorId = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE_SUB_ALLOCATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COURSE_SUB_ALLOCATION_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_COURSE_SUB_ALLOCATION_USER_SubInstructorId",
                        column: x => x.SubInstructorId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COURSE_TOPIC",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsArchieved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE_TOPIC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COURSE_TOPIC_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EXAMINATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: false),
                    ExamName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    TimeAllowed = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXAMINATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EXAMINATION_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "INSTRUCTOR_DEPARTMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseAllocationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INSTRUCTOR_DEPARTMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INSTRUCTOR_DEPARTMENT_COURSE_ALLOCATION_CourseAllocationId",
                        column: x => x.CourseAllocationId,
                        principalTable: "COURSE_ALLOCATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INSTRUCTOR_DEPARTMENT_DEPARTMENT_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DEPARTMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INSTRUCTOR_DEPARTMENT_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ASSIGNMENT_SUBMISSION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentInTextSubmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentSubmissionUploadLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentSubmissionHostedLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructorRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsGraded = table.Column<bool>(type: "bit", nullable: true),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    AssignmentId = table.Column<long>(type: "bigint", nullable: false),
                    CourseRegistrationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASSIGNMENT_SUBMISSION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASSIGNMENT_SUBMISSION_ASSIGNMENT_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "ASSIGNMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ASSIGNMENT_SUBMISSION_COURSE_REGISTRATION_CourseRegistrationId",
                        column: x => x.CourseRegistrationId,
                        principalTable: "COURSE_REGISTRATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COURSE_CONTENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveStream = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseTopicId = table.Column<long>(type: "bigint", nullable: false),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsArchieved = table.Column<bool>(type: "bit", nullable: false),
                    ContentTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE_CONTENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COURSE_CONTENT_COURSE_TOPIC_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "COURSE_TOPIC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OBJECTIVE_EXAMINATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExaminationId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorrectAnswer = table.Column<long>(type: "bigint", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    QuestionImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBJECTIVE_EXAMINATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBJECTIVE_EXAMINATION_ANSWER_OPTIONS_CorrectAnswer",
                        column: x => x.CorrectAnswer,
                        principalTable: "ANSWER_OPTIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OBJECTIVE_EXAMINATION_EXAMINATION_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "EXAMINATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT_EXAMINATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExaminationId = table.Column<long>(type: "bigint", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeTaken = table.Column<int>(type: "int", nullable: false),
                    StudentPersonId = table.Column<long>(type: "bigint", nullable: false),
                    LinkToTheoryAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT_EXAMINATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STUDENT_EXAMINATION_EXAMINATION_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "EXAMINATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_EXAMINATION_STUDENT_PERSON_StudentPersonId",
                        column: x => x.StudentPersonId,
                        principalTable: "STUDENT_PERSON",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "THEORY_EXAMINATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExaminationId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheoryQuestionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THEORY_EXAMINATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_THEORY_EXAMINATION_EXAMINATION_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "EXAMINATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QUESTION_OPTION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectiveExaminationId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerOptionsId = table.Column<long>(type: "bigint", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUESTION_OPTION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QUESTION_OPTION_ANSWER_OPTIONS_AnswerOptionsId",
                        column: x => x.AnswerOptionsId,
                        principalTable: "ANSWER_OPTIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QUESTION_OPTION_OBJECTIVE_EXAMINATION_ObjectiveExaminationId",
                        column: x => x.ObjectiveExaminationId,
                        principalTable: "OBJECTIVE_EXAMINATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT_EXAM_OBJECTIVE_ANSWER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentExaminationId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionOptionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT_EXAM_OBJECTIVE_ANSWER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STUDENT_EXAM_OBJECTIVE_ANSWER_QUESTION_OPTION_QuestionOptionId",
                        column: x => x.QuestionOptionId,
                        principalTable: "QUESTION_OPTION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_STUDENT_EXAM_OBJECTIVE_ANSWER_STUDENT_EXAMINATION_StudentExaminationId",
                        column: x => x.StudentExaminationId,
                        principalTable: "STUDENT_EXAMINATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANNOUNCEMENT_DepartmentId",
                table: "ANNOUNCEMENT",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ANNOUNCEMENT_UserId",
                table: "ANNOUNCEMENT",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ASSIGNMENT_CourseAllocationId",
                table: "ASSIGNMENT",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ASSIGNMENT_SUBMISSION_AssignmentId",
                table: "ASSIGNMENT_SUBMISSION",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ASSIGNMENT_SUBMISSION_CourseRegistrationId",
                table: "ASSIGNMENT_SUBMISSION",
                column: "CourseRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CLASS_MEETINGS_CourseAllocationId",
                table: "CLASS_MEETINGS",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CLASS_MEETINGS_UserId",
                table: "CLASS_MEETINGS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_LevelId",
                table: "COURSE",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_UserId",
                table: "COURSE",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_CourseId",
                table: "COURSE_ALLOCATION",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_CreatedById",
                table: "COURSE_ALLOCATION",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_InstructorId",
                table: "COURSE_ALLOCATION",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_LevelId",
                table: "COURSE_ALLOCATION",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_ALLOCATION_SessionSemesterId",
                table: "COURSE_ALLOCATION",
                column: "SessionSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_CONTENT_CourseTopicId",
                table: "COURSE_CONTENT",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_REGISTRATION_CourseAllocationId",
                table: "COURSE_REGISTRATION",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_REGISTRATION_SessionSemesterId",
                table: "COURSE_REGISTRATION",
                column: "SessionSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_REGISTRATION_StudentPersonId",
                table: "COURSE_REGISTRATION",
                column: "StudentPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_SUB_ALLOCATION_CourseAllocationId",
                table: "COURSE_SUB_ALLOCATION",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_SUB_ALLOCATION_SubInstructorId",
                table: "COURSE_SUB_ALLOCATION",
                column: "SubInstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_COURSE_TOPIC_CourseAllocationId",
                table: "COURSE_TOPIC",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_FacultySchoolId",
                table: "DEPARTMENT",
                column: "FacultySchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_HEADS_DepartmentId",
                table: "DEPARTMENT_HEADS",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTMENT_HEADS_UserId",
                table: "DEPARTMENT_HEADS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EXAMINATION_CourseAllocationId",
                table: "EXAMINATION",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GENERAL_AUDIT_UserId",
                table: "GENERAL_AUDIT",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_INSTRUCTOR_DEPARTMENT_CourseAllocationId",
                table: "INSTRUCTOR_DEPARTMENT",
                column: "CourseAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_INSTRUCTOR_DEPARTMENT_DepartmentId",
                table: "INSTRUCTOR_DEPARTMENT",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_INSTRUCTOR_DEPARTMENT_UserId",
                table: "INSTRUCTOR_DEPARTMENT",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OBJECTIVE_EXAMINATION_CorrectAnswer",
                table: "OBJECTIVE_EXAMINATION",
                column: "CorrectAnswer");

            migrationBuilder.CreateIndex(
                name: "IX_OBJECTIVE_EXAMINATION_ExaminationId",
                table: "OBJECTIVE_EXAMINATION",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_OTP_CODE_UserId",
                table: "OTP_CODE",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PERSON_GenderId",
                table: "PERSON",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_OPTION_AnswerOptionsId",
                table: "QUESTION_OPTION",
                column: "AnswerOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_OPTION_ObjectiveExaminationId",
                table: "QUESTION_OPTION",
                column: "ObjectiveExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_SEMESTER_SemesterId",
                table: "SESSION_SEMESTER",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_SESSION_SEMESTER_SessionId",
                table: "SESSION_SEMESTER",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_EXAM_OBJECTIVE_ANSWER_QuestionOptionId",
                table: "STUDENT_EXAM_OBJECTIVE_ANSWER",
                column: "QuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_EXAM_OBJECTIVE_ANSWER_StudentExaminationId",
                table: "STUDENT_EXAM_OBJECTIVE_ANSWER",
                column: "StudentExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_EXAMINATION_ExaminationId",
                table: "STUDENT_EXAMINATION",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_EXAMINATION_StudentPersonId",
                table: "STUDENT_EXAMINATION",
                column: "StudentPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PERSON_DepartmentId",
                table: "STUDENT_PERSON",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_STUDENT_PERSON_PersonId",
                table: "STUDENT_PERSON",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_THEORY_EXAMINATION_ExaminationId",
                table: "THEORY_EXAMINATION",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_PersonId",
                table: "USER",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_RoleId",
                table: "USER",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANNOUNCEMENT");

            migrationBuilder.DropTable(
                name: "ASSIGNMENT_SUBMISSION");

            migrationBuilder.DropTable(
                name: "CLASS_MEETINGS");

            migrationBuilder.DropTable(
                name: "COURSE_CONTENT");

            migrationBuilder.DropTable(
                name: "COURSE_SUB_ALLOCATION");

            migrationBuilder.DropTable(
                name: "DEPARTMENT_HEADS");

            migrationBuilder.DropTable(
                name: "GENERAL_AUDIT");

            migrationBuilder.DropTable(
                name: "INSTITUTION_TYPE");

            migrationBuilder.DropTable(
                name: "INSTRUCTOR_DEPARTMENT");

            migrationBuilder.DropTable(
                name: "OTP_CODE");

            migrationBuilder.DropTable(
                name: "PERSON_TYPE");

            migrationBuilder.DropTable(
                name: "SECURITY_QUESTION");

            migrationBuilder.DropTable(
                name: "STUDENT_EXAM_OBJECTIVE_ANSWER");

            migrationBuilder.DropTable(
                name: "THEORY_EXAMINATION");

            migrationBuilder.DropTable(
                name: "ASSIGNMENT");

            migrationBuilder.DropTable(
                name: "COURSE_REGISTRATION");

            migrationBuilder.DropTable(
                name: "COURSE_TOPIC");

            migrationBuilder.DropTable(
                name: "QUESTION_OPTION");

            migrationBuilder.DropTable(
                name: "STUDENT_EXAMINATION");

            migrationBuilder.DropTable(
                name: "OBJECTIVE_EXAMINATION");

            migrationBuilder.DropTable(
                name: "STUDENT_PERSON");

            migrationBuilder.DropTable(
                name: "ANSWER_OPTIONS");

            migrationBuilder.DropTable(
                name: "EXAMINATION");

            migrationBuilder.DropTable(
                name: "DEPARTMENT");

            migrationBuilder.DropTable(
                name: "COURSE_ALLOCATION");

            migrationBuilder.DropTable(
                name: "FACULTY_SCHOOL");

            migrationBuilder.DropTable(
                name: "COURSE");

            migrationBuilder.DropTable(
                name: "SESSION_SEMESTER");

            migrationBuilder.DropTable(
                name: "LEVEL");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "SEMESTER");

            migrationBuilder.DropTable(
                name: "SESSION");

            migrationBuilder.DropTable(
                name: "PERSON");

            migrationBuilder.DropTable(
                name: "ROLE");

            migrationBuilder.DropTable(
                name: "GENDER");
        }
    }
}
