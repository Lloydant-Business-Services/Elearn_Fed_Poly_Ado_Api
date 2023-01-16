using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ReportingService : IReportingSevice
    {
        private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;

        public ReportingService(ELearnContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        public async Task<IEnumerable<GetInstructorDto>> GetInstructors(long departmentId, long sessionSemesterId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            //return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemesterId == sessionSemesterId)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Select(f => new GetInstructorDto
                {
                    FullName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.UserId,
                    Email = f.User.Person.Email,
                    Department = f.Department,
                    CourseCode = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseCode : null,
                    CourseTitle = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseTitle : null,
                    CourseId = f.CourseAllocation != null ? f.CourseAllocation.CourseId : 0
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentReportListDto>> GetStudentsBy(long DepartmentId, long SessionSemesterId)
        {
            var getStudents = await _context.STUDENT_PERSON.Where(a => a.DepartmentId == DepartmentId)
                .Include(p => p.Person).ToListAsync();
            List<StudentReportListDto> returnList = new List<StudentReportListDto>();
            if (getStudents.Count() > 0)
            {
                foreach (var item in getStudents)
                {
                    var registeredStudent = await _context.COURSE_REGISTRATION.Where(a => a.StudentPersonId == item.Id && a.SessionSemesterId == SessionSemesterId)
                        .Include(s => s.StudentPerson)
                        .ThenInclude(p => p.Person)
                        .FirstOrDefaultAsync();
                    if (registeredStudent != null)
                    {
                        StudentReportListDto _student = new StudentReportListDto()
                        {
                            FullName = registeredStudent.StudentPerson.Person.Surname + " " + registeredStudent.StudentPerson.Person.Firstname + " " + registeredStudent.StudentPerson.Person.Othername,
                            MatricNumber = registeredStudent.StudentPerson.MatricNo,
                            email = registeredStudent.StudentPerson.Person.Email != null ? registeredStudent.StudentPerson.Person.Email : "-"
                        };
                        returnList.Add(_student);
                    }
                }
            }
            return returnList;
        }
        public async Task<GetSessionSemesterDto> GetActiveSessionSemester()
        {
            return await _context.SESSION_SEMESTER.Where(a => a.Active)
                .Include(s => s.Semester)
                .Include(s => s.Session)
                .Select(f => new GetSessionSemesterDto
                {
                    SemesterName = f.Semester.Name,
                    SessionName = f.Session.Name,
                    SemesterId = f.SemesterId,
                    SessionId = f.SessionId,
                    Id = f.Id
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AssignmentCumulativeDto>> AssignmentCumulativeScore(long courseId, long sessionSemesterId, long departmentId)
        {
            if (courseId == 0 && sessionSemesterId == 0)
                throw new NullReferenceException("No sessionId or courseId was recieved");
            List<AssignmentCumulativeDto> cumulativeList = new List<AssignmentCumulativeDto>();
            var getCourseAllocation = await _context.COURSE_ALLOCATION.Where(x => x.CourseId == courseId && x.SessionSemesterId == sessionSemesterId).FirstOrDefaultAsync();
            var courseStudents = await _context.COURSE_REGISTRATION.Where(x => x.CourseAllocationId == getCourseAllocation.Id && x.StudentPerson.DepartmentId == departmentId)
                .Include(p => p.StudentPerson)
                .ThenInclude(s => s.Person)
                .ToListAsync();

            if (courseStudents != null && courseStudents.Count > 0)
            {
                foreach (var item in courseStudents)
                {
                    var assignmentSubmission = await _context.ASSIGNMENT_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id).ToListAsync();
                    if (assignmentSubmission != null && assignmentSubmission.Count > 0)
                    {
                        AssignmentCumulativeDto dto = new AssignmentCumulativeDto()
                        {
                            StudentName = item.StudentPerson.Person.Surname + " " + item.StudentPerson.Person.Surname,
                            CumulativeScore = assignmentSubmission.Sum(x => x.Score),
                            MatricNumber = item.StudentPerson.MatricNo
                        };
                        cumulativeList.Add(dto);
                    }
                }
            }

            return cumulativeList;
        }

        public async Task<IEnumerable<ComprehensiveAssignmentCumulativeDto>> ComprehensiveAssignmentReport(long courseId, long sessionSemesterId, long departmentId)
        {
            if (courseId == 0 && sessionSemesterId == 0)
                throw new NullReferenceException("No sessionId or courseId was recieved");
            List<ComprehensiveAssignmentCumulativeDto> cumulativeList = new List<ComprehensiveAssignmentCumulativeDto>();
            var getCourseAllocation = await _context.COURSE_ALLOCATION.Where(x => x.CourseId == courseId && x.SessionSemesterId == sessionSemesterId).FirstOrDefaultAsync();
            var courseStudents = await _context.COURSE_REGISTRATION.Where(x => x.CourseAllocationId == getCourseAllocation.Id && x.StudentPerson.DepartmentId == departmentId)
                .Include(p => p.StudentPerson)
                .ThenInclude(s => s.Person)
                .ToListAsync();

            if (courseStudents != null && courseStudents.Count > 0)
            {
                foreach (var item in courseStudents)
                {
                    var assignmentSubmission = await _context.ASSIGNMENT_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id)
                        .Select(f => new DetailDto()
                        {
                            AssignmentName = f.Assignment.AssignmentName,
                            Score = f.Score
                        })
                        .ToListAsync();
                    if (assignmentSubmission != null && assignmentSubmission.Count > 0)
                    {
                        ComprehensiveAssignmentCumulativeDto dto = new ComprehensiveAssignmentCumulativeDto()
                        {
                            StudentName = item.StudentPerson.Person.Surname + " " + item.StudentPerson.Person.Surname,
                            CumulativeScore = assignmentSubmission.Sum(x => x.Score),
                            MatricNumber = item.StudentPerson.MatricNo,
                            DetailList = assignmentSubmission
                        };
                        cumulativeList.Add(dto);
                    }
                }
            }

            return cumulativeList;
        }

        public async Task<IEnumerable<ComprehensiveAssignmentCumulativeDto>> ComprehensiveQuizReport(long courseId, long sessionSemesterId, long departmentId)
        {
            if (courseId == 0 && sessionSemesterId == 0)
                throw new NullReferenceException("No sessionId or courseId was recieved");
            List<ComprehensiveAssignmentCumulativeDto> cumulativeList = new List<ComprehensiveAssignmentCumulativeDto>();
            var getCourseAllocation = await _context.COURSE_ALLOCATION.Where(x => x.CourseId == courseId && x.SessionSemesterId == sessionSemesterId).FirstOrDefaultAsync();
            var courseStudents = await _context.COURSE_REGISTRATION.Where(x => x.CourseAllocationId == getCourseAllocation.Id && x.StudentPerson.DepartmentId == departmentId)
                .Include(p => p.StudentPerson)
                .ThenInclude(s => s.Person)
                .ToListAsync();

            if (courseStudents != null && courseStudents.Count > 0)
            {
                foreach (var item in courseStudents)
                {
                    var assignmentSubmission = await _context.QUIZ_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id)
                        .Select(f => new DetailDto()
                        {
                            AssignmentName = f.Quiz.QuizName,
                            Score = f.Score
                        })
                        .ToListAsync();
                    if (assignmentSubmission != null && assignmentSubmission.Count > 0)
                    {
                        ComprehensiveAssignmentCumulativeDto dto = new ComprehensiveAssignmentCumulativeDto()
                        {
                            StudentName = item.StudentPerson.Person.Surname + " " + item.StudentPerson.Person.Surname,
                            CumulativeScore = assignmentSubmission.Sum(x => x.Score),
                            MatricNumber = item.StudentPerson.MatricNo,
                            DetailList = assignmentSubmission
                        };
                        cumulativeList.Add(dto);
                    }
                }
            }

            return cumulativeList;
        }

        public async Task<StudentCumulativeAssignmentModel> CumulativeComprehensiveAssignmentReportByStudent(long personId, long sessionSemesterId)
        {
            StudentCumulativeAssignmentModel student = new StudentCumulativeAssignmentModel();
            List<AssignmentCumulativeModel> StudentAssignmentModel = new List<AssignmentCumulativeModel>();
            List<AssignmentSubmission> assignmentDetails = await _context.ASSIGNMENT_SUBMISSION.Where(x => x.CourseRegistration.StudentPerson.PersonId == personId && x.Assignment.PublishResult && x.CourseRegistration.SessionSemesterId == sessionSemesterId
            )
                .Include(x => x.CourseRegistration)
                .ThenInclude(x => x.StudentPerson)
                .ThenInclude(x => x.Person)
                .Include(x => x.CourseRegistration)
                .ThenInclude(x => x.SessionSemester)
                .Include(x => x.CourseRegistration)
                .ThenInclude(x => x.CourseAllocation)
                .ThenInclude(x => x.Course)
                .Include(x => x.Assignment)
                .ToListAsync();


            if (assignmentDetails.Any())
            {
                var studentInfo = assignmentDetails[0].CourseRegistration;
                foreach (var assignment in assignmentDetails)
                {

                    AssignmentCumulativeModel reportDetails = new AssignmentCumulativeModel()
                    {
                        CourseCode = assignment.CourseRegistration.CourseAllocation.Course.CourseCode,
                        CourseTitle = assignment.CourseRegistration.CourseAllocation.Course.CourseTitle,
                        Score = assignment.Score
                    };

                    StudentAssignmentModel.Add(reportDetails);
                }
                student = new StudentCumulativeAssignmentModel()
                {
                    StudentName = studentInfo.StudentPerson.Person.Surname + " " + studentInfo.StudentPerson.Person.Firstname,
                    MatricNumber = studentInfo.StudentPerson.MatricNo,
                    StudentAssignmentModel = StudentAssignmentModel,
                    CumulativeScore = StudentAssignmentModel.Sum(x => x.Score)
                };
               
            }
            return student;
        }





    }
}
