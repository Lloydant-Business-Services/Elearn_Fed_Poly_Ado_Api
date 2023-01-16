using BusinessLayer.Interface;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Enums;
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
    public class InstructorHodService : IInstructorHodService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string defualtPassword = "1234567";
        private readonly IEmailService _emailService;

        public InstructorHodService(IConfiguration configuration, ELearnContext context, IEmailService emailService)
        {
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
        }
        public async Task<ResponseModel> AddCourseInstructorAndHod(AddUserDto userDto)
        {
            ResponseModel response = new ResponseModel();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                User user = new User();
                var doesEmailExist = await _context.USER.Where(x => x.Person.Email == userDto.Email).Include(x => x.Person).FirstOrDefaultAsync();
                if(doesEmailExist != null)
                {
                    response.Message = "Email already exists";
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    return response;
                }
                if (userDto.DepartmentId <= 0 && userDto.RoleId != (int)Roles.SubAdmin)
                {
                    response.Message = "Department not Specified";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
                
                Person person = new Person()
                {
                    Surname = userDto.Surname,
                    Firstname = userDto.Firstname,
                    Othername = userDto.Othername,
                    Email = userDto.Email,
                    Active = true
                };
                _context.Add(person);
                await _context.SaveChangesAsync();

                Utility.CreatePasswordHash(defualtPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.Username = userDto.Email;
                user.RoleId = userDto.RoleId;
                user.IsVerified = true;
                user.Active = true;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PersonId = person.Id;
                _context.Add(user);
                await _context.SaveChangesAsync();

                if (person.Email != null)
                {
                    EmailDto emailDto = new EmailDto()
                    {
                        ReceiverEmail = person.Email,
                        ReceiverName = person.Firstname,
                        Password = "1234567",
                        RegNumber = user.Username,
                        Subject = "Account Creation Notification",
                        NotificationCategory = EmailNotificationCategory.AccountAdded

                    };
                    await _emailService.EmailFormatter(emailDto);
                }
                if (userDto.RoleId == (int)Roles.DepartmentAdministrator)
                {
                    var doesExist = await _context.DEPARTMENT_HEADS.Where(d => d.DepartmentId == userDto.DepartmentId && d.Active).FirstOrDefaultAsync();
                    if (doesExist != null)
                    {
                        response.Message = "HOD already exists for the selected department";
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        return response;
                    }
                    DepartmentHeads departmentHead = new DepartmentHeads()
                    {
                        DepartmentId = userDto.DepartmentId,
                        UserId = user.Id,
                        Active = true
                    };
                    _context.Add(departmentHead);
                    await _context.SaveChangesAsync();
                }

                else if (userDto.RoleId == (int)Roles.Instructor)
                {
                    
                    InstructorDepartment instructorDepartment = new InstructorDepartment()
                    {
                        //CourseAllocation = courseAllocation != null ? courseAllocation : null,
                        DepartmentId = userDto.DepartmentId,
                        UserId = user.Id
                    };
                    _context.Add(instructorDepartment);
                    await _context.SaveChangesAsync();
                }
                else if (userDto.RoleId == (int)Roles.SubAdmin)
                {

                    SubAdmin subAdmin = new SubAdmin()
                    {
                        //CourseAllocation = courseAllocation != null ? courseAllocation : null,
                        UserId = user.Id,
                        Active = true,
                        DateAdded = DateTime.Now,
                    };
                    _context.Add(subAdmin);
                    await _context.SaveChangesAsync();
                }


                await transaction.CommitAsync();

                return response;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

        }
        public async Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesByUserId(long instructorUserId)
        {
            try
            {
                List<InstructorCoursesDto> dtoList = new List<InstructorCoursesDto>();
                var instructorCourses = await _context.COURSE_ALLOCATION
                    .Include(c => c.Course)
                    .Include(l => l.Level)
                    .Where(x => x.InstructorId == instructorUserId && x.SessionSemester.Active)
                    .ToListAsync();

                foreach (var item in instructorCourses)
                {
                    InstructorCoursesDto dto = new InstructorCoursesDto();
                    var isRegistered = await _context.COURSE_REGISTRATION.Where(s => s.SessionSemester.Active && s.CourseAllocationId == item.Id).ToListAsync();
                    dto.CourseAllocationId = item.Id;
                    dto.CourseTitle = item.Course.CourseTitle;
                    dto.CourseCode = item.Course.CourseCode;
                    dto.CourseId = item.Course.Id;
                    dto.Level = item?.Level?.Name;
                    dto.RegisteredStudents = isRegistered.Count();

                    dtoList.Add(dto);
                }
               
                return dtoList.OrderBy(a => a.CourseTitle);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors()
        {
            try
            {
                return await _context.INSTRUCTOR_DEPARTMENT.Where(f => f.Id > 0)
                .Include(p => p.User)
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
                    CourseTitle = f.CourseAllocation !=null ? f.CourseAllocation.Course.CourseTitle : null,
                    CourseId = f.CourseAllocation !=null ? f.CourseAllocation.CourseId : 0,
                    Level = f.CourseAllocation != null ? f.CourseAllocation.Level.Name : null

                })
                .OrderBy(x => x.FullName)
                .ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructorsAndHodPerson(string searchInput)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchInput))
                {
                    return await _context.USER.Where(f => (f.RoleId == (int)SystemRole.Instructor || f.RoleId == (int)SystemRole.HOD) && (f.Person.Firstname.Contains(searchInput.ToLower()) || f.Person.Surname.Contains(searchInput.ToLower())))
                    .Include(p => p.Person)
                   .Select(f => new GetInstructorDto
                   {
                       FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                       UserId = f.Id,
                       Email = f.Person.Email,
                   })
                   .OrderByDescending(x => x.UserId)
                   .ToListAsync();
                }
                else
                {
                    return await _context.USER.Where(f => f.RoleId == (int)SystemRole.Instructor || f.RoleId == (int)SystemRole.HOD)
                   .Include(p => p.Person)
                   .Select(f => new GetInstructorDto
                   {
                       FullName = f.Person.Surname + " " + f.Person.Firstname + " " + f.Person.Othername,
                       UserId = f.Id,
                       Email = f.Person.Email,
                   })
                   .OrderByDescending(x => x.UserId)
                   .Take(50)
                   .ToListAsync();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<GetInstructorDto>> GetAllDepartmentHeads()
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            return await _context.DEPARTMENT_HEADS.Where(d => d.Id > 0)
                .Include(p => p.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Select(f => new GetInstructorDto
                {
                    FullName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    UserId = f.Id,
                    Email = f.User.Person.Email,
                    Department = f.Department,
                })
                .ToListAsync();
        }
        

        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByDepartmentId(long departmentId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            //return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Id == activeSessionSemester.Id)
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.SessionSemester)
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

        public async Task<IEnumerable<GetInstructorDto>> GetAllInstructors()
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            //return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Id == activeSessionSemester.Id)
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.Active == true)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.SessionSemester)
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
        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByFacultyId(long facultyId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            //return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.Department.FacultySchoolId == facultyId && d.CourseAllocation.SessionSemester.Id == activeSessionSemester.Id)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.SessionSemester)
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

        public async Task<HODDashboardSummaryDto> HODDashboardSummary(long DepartmentId)
        {
            HODDashboardSummaryDto summary = new HODDashboardSummaryDto();
            long courseCount = 0;
            long courseMaterialCount = 0;
            var studentDept = await _context.STUDENT_PERSON.Where(d => d.DepartmentId == DepartmentId).ToListAsync();
            var departmentInstructors = await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == DepartmentId && d.CourseAllocation.SessionSemester.Active).Include(c => c.CourseAllocation)
                .ToListAsync();

            foreach(var item in departmentInstructors)
            {
                var courseAllocation = await _context.COURSE_ALLOCATION.Where(x => x.InstructorId == item.CourseAllocation.InstructorId)
                    .ToListAsync();
                var courseTopic = await _context.COURSE_TOPIC.Where(x => x.CourseAllocation.InstructorId == item.CourseAllocation.InstructorId)
                    .Include(c => c.CourseAllocation)
                    .ToListAsync();
                if(courseAllocation.Count > 0)
                {
                    courseCount += courseAllocation.Count();
                }
                if(courseTopic.Count > 0)
                {
                    courseMaterialCount += courseTopic.Count();
                }
            }

            summary.AllDepartmentInstructors = departmentInstructors.Count();
            summary.AllDepartmentCourses = courseCount;
            summary.AllDepartmentStudents = studentDept.Count();
            summary.AllDepartmentCourseMaterials = courseMaterialCount;
            return summary;
        }
        public async Task<InstructorSummaryDto> InstructorDashboardSummary(long InstructorId)
        {
            InstructorSummaryDto summary = new InstructorSummaryDto();
            long StudentCount = 0;

            var courseAllocation = await _context.COURSE_ALLOCATION.Where(x => x.InstructorId == InstructorId && x.SessionSemester.Active).ToListAsync();
            var _assignments = await _context.ASSIGNMENT.Where(c => c.CourseAllocation.InstructorId == InstructorId).ToListAsync();



            foreach (var item in courseAllocation)
            {
                var isRegistered = await _context.COURSE_REGISTRATION.Where(s => s.SessionSemester.Active && s.CourseAllocationId == item.Id).ToListAsync();
                
                if (isRegistered.Count > 0)
                {
                    StudentCount += courseAllocation.Count();
                }
            
            }

            summary.Students = StudentCount;
            summary.Courses = courseAllocation.Count();
            summary.Assignments = _assignments.Count();
            return summary;
        }


        public async Task<bool> RemoveHod(long DepartmentId)
        {
            try
            {
                var getHod = await _context.DEPARTMENT_HEADS.Where(x => x.DepartmentId == DepartmentId).ToListAsync();
                if(getHod != null && getHod.Count > 0)
                {
                    foreach(var item in getHod)
                    {
                        _context.Remove(item);
                        await _context.SaveChangesAsync();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> RemoveInstructor(long userId)
        {
            try
            {
                var isAllocated = await _context.COURSE_ALLOCATION.Where(x => x.InstructorId == userId).FirstOrDefaultAsync();
                if (isAllocated != null)
                    throw new Exception("Instructor has a course assigned");
                var instructor = await _context.INSTRUCTOR_DEPARTMENT.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                var user = await _context.USER.Where(x => x.Id == userId).FirstOrDefaultAsync();
                var person = await _context.PERSON.Where(x => x.Id == user.PersonId).FirstOrDefaultAsync();
                if (instructor != null)
                {
                        _context.Remove(instructor);
                        await _context.SaveChangesAsync();
                    //return true;
                }
                if (user != null)
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                }
                if (person != null)
                {
                    _context.Remove(person);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
