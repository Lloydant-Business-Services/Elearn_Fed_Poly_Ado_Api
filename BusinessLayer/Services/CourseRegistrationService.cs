using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CourseRegistrationService : ICourseRegistrationService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;

        public CourseRegistrationService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseModel> RegisterCourseSingle(RegisterCourseSingleDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                var activeSessionSemester = await GetActiveSessionSemester();
                var getPerson = await _context.PERSON.Where(p => p.Id == dto.PersonId).FirstOrDefaultAsync();
                var getStudentPerson = await _context.STUDENT_PERSON.Where(s => s.PersonId == getPerson.Id).FirstOrDefaultAsync();

             
                    //var _allocationId = Convert.ToInt32(item);
                    var isRegistered = await _context.COURSE_REGISTRATION.Where(s => s.StudentPersonId == getStudentPerson.Id && s.SessionSemesterId == activeSessionSemester.Id && s.CourseAllocationId == dto.CourseAllocationId).FirstOrDefaultAsync();
                    if (isRegistered != null)
                    {
                        response.Message = "Course already registered for the active session semester";
                        response.StatusCode = StatusCodes.Status208AlreadyReported;
                        return response;
                    }
                
                    CourseRegistration courseRegistration = new CourseRegistration()
                    {
                        StudentPersonId = getStudentPerson.Id,
                        SessionSemesterId = activeSessionSemester.Id,
                        CourseAllocationId = dto.CourseAllocationId,
                        DateRegistered = DateTime.Now,
                        Active = true
                    };
                    _context.Add(courseRegistration);
                    await _context.SaveChangesAsync();
                
               
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel> RegisterCourses(RegisterCourseDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                var activeSessionSemester = await GetActiveSessionSemester();
                var getPerson = await _context.PERSON.Where(p => p.Id == dto.PersonId).FirstOrDefaultAsync();
                var getStudentPerson = await _context.STUDENT_PERSON.Where(s => s.PersonId == getPerson.Id).FirstOrDefaultAsync();

                foreach (var item in dto.CourseAllocation)
                {
                    //var _allocationId = Convert.ToInt32(item);
                    var isRegistered = await _context.COURSE_REGISTRATION.Where(s => s.StudentPersonId == getStudentPerson.Id && s.SessionSemesterId == activeSessionSemester.Id && s.CourseAllocationId == item.CourseAllocationId).FirstOrDefaultAsync();
                    if (isRegistered != null)
                    {
                        response.Message = "Course already registered for the active session semester";
                        response.StatusCode = StatusCodes.Status208AlreadyReported;
                        return response;
                    }

                    CourseRegistration courseRegistration = new CourseRegistration()
                    {
                        StudentPersonId = getStudentPerson.Id,
                        SessionSemesterId = activeSessionSemester.Id,
                        CourseAllocationId = item.CourseAllocationId,
                        DateRegistered = DateTime.Now,
                        Active = true
                    };
                    _context.Add(courseRegistration);
                    await _context.SaveChangesAsync();
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<GetRegisteredCoursesDto>> GetRegisteredCourses(long personId, long sessionSemesterId)
        {
            var getPerson = await _context.STUDENT_PERSON.Where(s => s.PersonId == personId).FirstOrDefaultAsync();
            var courseRegistartion = await _context.COURSE_REGISTRATION.Where(f => f.StudentPersonId == getPerson.Id && f.SessionSemesterId == sessionSemesterId)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(p => p.Instructor)
                .ThenInclude(f => f.Person)
                .Select(f => new GetRegisteredCoursesDto {
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseLecturer = f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername,
                    CourseId = f.CourseAllocation.CourseId,
                    CourseAllocationId = f.CourseAllocation.Id,
                    DateRegistered = f.DateRegistered
                })
                .ToListAsync();
            return courseRegistartion;
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
    }
}
