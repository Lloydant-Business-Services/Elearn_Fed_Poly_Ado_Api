using BusinessLayer.Interface;
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
    public class CourseService : ICourseService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;

        public CourseService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseModel> AddCourse(AddCourseDto courseDto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                var courseCodeSlug = Utility.GenerateSlug(courseDto.CourseCode);
                var courseTitleSlug = Utility.GenerateSlug(courseDto.CourseTitle);

                User user = await _context.USER.Where(s => s.Id == courseDto.UserId).FirstOrDefaultAsync();
                Course doesCourseExist = await _context.COURSE.Where(c => c.CourseCodeSlug == courseCodeSlug && c.CourseTitleSlug == courseTitleSlug).FirstOrDefaultAsync();
                if (user == null)
                    throw new NullReferenceException("User was not found");
                if (doesCourseExist != null)
                {
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    response.Message = "Course Already Added";
                    return response;
                }
                Course course = new Course()
                {
                    CourseTitle = courseDto.CourseTitle,
                    CourseCode = courseDto.CourseCode,
                    CourseCodeSlug = Utility.GenerateSlug(courseDto.CourseCode),
                    CourseTitleSlug = Utility.GenerateSlug(courseDto.CourseTitle),
                    DateCreated = DateTime.Now,
                    UserId = courseDto.UserId,
                    //LevelId = courseDto.LevelId,
                    Active = true
                };
                _context.Add(course);
                await _context.SaveChangesAsync();
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "success";
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CoursePloadSheetAggregation> ProcessBulkCourseUpload(IEnumerable<CourseUploadModel> courseList, long userId)
        {
            CoursePloadSheetAggregation uploadAggregation = new CoursePloadSheetAggregation();
            List<CourseUploadModel> failedUploads = new List<CourseUploadModel>();
            uploadAggregation.SuccessfullUpload = 0;
            uploadAggregation.FailedUpload = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (courseList.Count() > 0)
                {
                    foreach (CourseUploadModel course in courseList)
                    {
                        if (string.IsNullOrEmpty(course.CourseTitle) || string.IsNullOrEmpty(course.CourseCode))
                        {
                            continue;
                        }
                        var codeSlug = Utility.GenerateSlug(course.CourseCode);
                        var titleSlug = Utility.GenerateSlug(course.CourseTitle);
                        CourseUploadModel failedUploadSingle = new CourseUploadModel();

                        var doesCourseCodeAlreadyExist = await _context.COURSE.Where(x => x.CourseCodeSlug == codeSlug).FirstOrDefaultAsync();

                        if (doesCourseCodeAlreadyExist == null)
                        {
                            Course courseEntity = new Course()
                            {
                                CourseTitle = course.CourseTitle,
                                CourseCode = course.CourseCode,
                                CourseCodeSlug = codeSlug,
                                CourseTitleSlug = titleSlug,
                                DateCreated = DateTime.Now,
                                Active = true,
                                UserId = userId
                            };
                            _context.Add(courseEntity);
                            await _context.SaveChangesAsync();

                            uploadAggregation.SuccessfullUpload += 1;

                        }
                        //Already exists
                        else
                        {
                            failedUploadSingle.CourseCode = course.CourseCode;
                            failedUploadSingle.CourseTitle = course.CourseTitle;
                            failedUploads.Add(failedUploadSingle);
                            uploadAggregation.FailedUpload += 1;
                        }

                    }
                    await transaction.CommitAsync();
                    uploadAggregation.FailedCourseUploads = failedUploads;

                }
                return uploadAggregation;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public async Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                Course course = await _context.COURSE.Where(c => c.Id == dto.Id).FirstOrDefaultAsync();
                if (course == null)
                    throw new NullReferenceException("Course not found");
                course.CourseTitle = dto.CourseTitle != null ? dto.CourseTitle : course.CourseTitle;
                course.CourseCode = dto.CourseCode != null ? dto.CourseCode : course.CourseCode;
                _context.Update(course);
                await _context.SaveChangesAsync();
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "success";
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<AddCourseDto>> GetStudentCourses()
        {
            return await _context.COURSE_ALLOCATION.Where(a => a.SessionSemester.Active)
                .Include(x => x.Department)
                .Select(f => new AddCourseDto
                {
                    CourseCode = f.Course.CourseCode,
                    CourseTitle = f.Course.CourseTitle + " ("+f.Department.Name+")",
                    UserId = f.Course.UserId,
                    DateCreated = f.Course.DateCreated,
                    Id = f.Course.Id,
                    LevelId = f.LevelId,
                    CourseAllocationId = f.Id
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<AddCourseDto>> GetStudentCoursesByDepartment(long departmentId)
        {
            return await _context.COURSE_ALLOCATION.Where(a => a.SessionSemester.Active && a.DepartmentId == departmentId)
                .Include(x => x.Department)
                .Select(f => new AddCourseDto
                {
                    CourseCode = f.Course.CourseCode,
                    CourseTitle = f.Course.CourseTitle,
                    UserId = f.Course.UserId,
                    DateCreated = f.Course.DateCreated,
                    Id = f.Course.Id,
                    LevelId = f.LevelId,
                    CourseAllocationId = f.Id
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<AllCoursesDto>> GetCourses()
        {
            
           // List<AllCoursesDto> courseDtoList = new List<AllCoursesDto>();

            return await _context.COURSE.Where(a => a.Active)
                .Select(f => new AllCoursesDto
                { 
                    CourseCode = f.CourseCode,
                    CourseTitle = f.CourseTitle,
                    UserId = f.UserId,
                    DateCreated = f.DateCreated,
                    Id = f.Id,
                    CourseDetail = f.CourseCode + " - " + f.CourseTitle
                    //LevelId = f.LevelId,
                })
                .ToListAsync();
            
            
        }
        public async Task<AllCoursesDto> GetCourseByCourseId(long courseId)
        {
            return await _context.COURSE.Where(c => c.Id == courseId)
                .Select(f => new AllCoursesDto
                {
                    CourseTitle = f.CourseTitle,
                    CourseCode = f.CourseCode,
                    DateCreated = f.DateCreated,
                    Id = f.Id
                })
                .FirstOrDefaultAsync();
        }
        public async Task<AllCoursesDto> AllocatedCourseByAllocationId(long courseAllocationId)
        {
            return await _context.COURSE_ALLOCATION.Where(c => c.Id == courseAllocationId)
                .Select(f => new AllCoursesDto
                {
                    CourseTitle = f.Course.CourseTitle,
                    CourseCode = f.Course.CourseCode,
                    DateCreated = f.Course.DateCreated,
                    Id = f.Course.Id,
                    CourseAllocationId = f.Id
                })
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<GetDepartmentCourseDto>> GetDepartmentalCourses(long departmentId)
        {
            var activeSessionSemester = await GetActiveSessionSemester();
            return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
                .Include(d => d.User)
                .ThenInclude(p => p.Person)
                .Include(c => c.CourseAllocation)
                .ThenInclude(s => s.SessionSemester)
                .Include(c => c.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Select(f => new GetDepartmentCourseDto
                {
                    CourseLecturer = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    CourseId = f.CourseAllocation.Course.Id,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    CourseAllocationId = f.CourseAllocation.Id,
                    InstructorUserId = f.UserId
                    
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<GetDepartmentCourseDto>> GetAllocatedCoursesByDepartment(long departmentId)
        {
            List<GetDepartmentCourseDto> returnList = new List<GetDepartmentCourseDto>();
            var activeSessionSemester = await GetActiveSessionSemester();
            var Instructors =  await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemester.Active)
                .ToListAsync();
            if (Instructors.Any())
            {
                for(var i = 0; i < Instructors.Count(); i++)
                {
                    var allocatedCourses = await _context.COURSE_ALLOCATION.Where(x => x.InstructorId == Instructors[i].UserId)
                         .Include(d => d.Instructor)
                        .ThenInclude(p => p.Person)
                        .Include(s => s.SessionSemester)
                        .Include(c => c.Course)
                        .Include(d => d.Department)
                        .Select(f => new GetDepartmentCourseDto {
                            CourseLecturer = f.Instructor.Person.Surname + " " + f.Instructor.Person.Firstname + " " + f.Instructor.Person.Othername,
                            CourseId = f.Course.Id,
                            CourseCode = f.Course.CourseCode,
                            CourseTitle = f.Course.CourseTitle,
                            CourseAllocationId = f.Id,
                            InstructorUserId = f.InstructorId,
                            DepartmentName = f.Department.Name
                            
                        })
                        .ToListAsync();
                    if (allocatedCourses.Any())
                    {
                        returnList.AddRange(allocatedCourses);
                    }
                }
            }

            return returnList;
        }

        public async Task<IEnumerable<GetDepartmentCourseDto>> GetAllocatedServiceCourses()
        {
            List<GetDepartmentCourseDto> returnList = new List<GetDepartmentCourseDto>();
            
                   return await _context.COURSE_ALLOCATION.Where(x => x.CreatedBy.RoleId == (long)SystemRole.ServiceAdmin && x.SessionSemester.Active)
                         .Include(d => d.Instructor)
                        .ThenInclude(p => p.Person)
                        .Include(s => s.SessionSemester)
                        .Include(c => c.Course)
                        .Include(d => d.Department)
                        .Include(x => x.CreatedBy)
                        .Select(f => new GetDepartmentCourseDto
                        {
                            CourseLecturer = f.Instructor.Person.Surname + " " + f.Instructor.Person.Firstname + " " + f.Instructor.Person.Othername,
                            CourseId = f.Course.Id,
                            CourseCode = f.Course.CourseCode,
                            CourseTitle = f.Course.CourseTitle,
                            CourseAllocationId = f.Id,
                            InstructorUserId = f.InstructorId,
                            DepartmentName = f.Department.Name

                        })
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
    }
}
