using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class ELearnContext:DbContext
    {
        public ELearnContext(DbContextOptions<ELearnContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(f => f.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }

        public DbSet<Gender> GENDER { get; set; }
        public DbSet<Person> PERSON { get; set; }
        public DbSet<User> USER { get; set; }
        public DbSet<AdminDelegations> ADMIN_DELEGATIONS { get; set; }
        public DbSet<SubAdmin> SUB_ADMIN { get; set; }
        public DbSet<Role> ROLE { get; set; }
        public DbSet<SecurityQuestion> SECURITY_QUESTION { get; set; }
        public DbSet<Session> SESSION { get; set; }
        public DbSet<Level> LEVEL { get; set; }
        public DbSet<Semester> SEMESTER { get; set; }
        public DbSet<SessionSemester> SESSION_SEMESTER { get; set; }
        public DbSet<FacultySchool> FACULTY_SCHOOL { get; set; }
        public DbSet<Department> DEPARTMENT { get; set; }
        public DbSet<InstitutionType> INSTITUTION_TYPE { get; set; }
        public DbSet<PersonType> PERSON_TYPE { get; set; }
        public DbSet<StudentPerson> STUDENT_PERSON { get; set; }
        public DbSet<Course> COURSE { get; set; }
        public DbSet<CourseTopic> COURSE_TOPIC { get; set; }
        public DbSet<CourseContent> COURSE_CONTENT { get; set; }
        public DbSet<CourseAllocation> COURSE_ALLOCATION { get; set; }
        public DbSet<CourseRegistration> COURSE_REGISTRATION { get; set; }
        public DbSet<CourseSubAllocation> COURSE_SUB_ALLOCATION { get; set; }
        public DbSet<Assignment> ASSIGNMENT { get; set; }
        public DbSet<AssignmentSubmission> ASSIGNMENT_SUBMISSION { get; set; }
        public DbSet<AnswerOptions> ANSWER_OPTIONS { get; set; }
        public DbSet<Examination> EXAMINATION { get; set; }
        public DbSet<ObjectiveExamination> OBJECTIVE_EXAMINATION { get; set; }
        public DbSet<StudentExamination> STUDENT_EXAMINATION { get; set; }
        public DbSet<QuestionOption> QUESTION_OPTION { get; set; }
        public DbSet<StudentExamObjectiveAnswer> STUDENT_EXAM_OBJECTIVE_ANSWER { get; set; }
        public DbSet<TheoryExamination> THEORY_EXAMINATION { get; set; }
        public DbSet<GeneralAudit> GENERAL_AUDIT { get; set; }
        public DbSet<InstructorDepartment> INSTRUCTOR_DEPARTMENT { get; set; }
        public DbSet<DepartmentHeads> DEPARTMENT_HEADS { get; set; }
        public DbSet<Announcement> ANNOUNCEMENT { get; set; }
        public DbSet<ClassMeetings> CLASS_MEETINGS { get; set; }
        public DbSet<Otp_Code> OTP_CODE { get; set; }
        public DbSet<Quiz> QUIZ { get; set; }
        public DbSet<QuizSubmission> QUIZ_SUBMISSION { get; set; }
        public DbSet<StudentPayment> STUDENT_PAYMENT { get; set; }
        public DbSet<NotificationTracker> NOTIFICATION_TRACKER { get; set; }
        public DbSet<PaymentSetup> PAYMENT_SETUP { get; set; }
        public DbSet<AllocationDepartmentLog> ALLOCATION_DEPARTMENT_LOG { get; set; }
        public DbSet<RwLog> RWLOG { get; set; }
        public DbSet<Ratings> RATINGS { get; set; }

    }
}
