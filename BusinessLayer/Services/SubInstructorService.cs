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
using static DataLayer.Dtos.AddSubInstructorDto;

namespace BusinessLayer.Services
{
    public class SubInstructorService : ISubInstructorService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string baseUrl;
        //private readonly IFileUpload _fileUpload;

        public SubInstructorService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
            //_fileUpload = fileUpload;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        public async Task<ResponseModel> AddSubInstructor(AddSubInstructorDto addSubInstructorDto)
        {
            ResponseModel response = new ResponseModel();
            if (addSubInstructorDto.UserId == 0)
                throw new NullReferenceException("No UserId provided");
            if (addSubInstructorDto.CourseAllocationId == 0)
                throw new NullReferenceException("No CourseId was provided");
            if (addSubInstructorDto != null)
            {
                var isAlreadyAssigned = await _context.COURSE_SUB_ALLOCATION.Where(x => x.SubInstructorId == addSubInstructorDto.UserId && x.CourseAllocationId == addSubInstructorDto.CourseAllocationId).FirstOrDefaultAsync();

                if (isAlreadyAssigned != null)
                {
                    response.Message = "Sub instructor already assigned";
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    return response;
                }

                CourseSubAllocation institutionSubInstructors = new CourseSubAllocation()
                {
                    SubInstructorId = addSubInstructorDto.UserId,
                    CourseAllocationId = addSubInstructorDto.CourseAllocationId,
                    DateCreated = DateTime.Now,
                    Active = true
                };
                _context.Add(institutionSubInstructors);
                await _context.SaveChangesAsync();
                response.Message = "success";
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            return null;

        }

        public async Task<IEnumerable<GetSubInstructorDto>> GetCourseSubIntructors(long courseId)
        {

            List<GetSubInstructorDto> subinstructors_list = new List<GetSubInstructorDto>();
            if (courseId == 0)
                throw new NullReferenceException("No CourseId was provided");
            var subInstructors = await _context.COURSE_SUB_ALLOCATION.Where(c => c.CourseAllocation.CourseId == courseId && c.CourseAllocation.SessionSemester.Active)
                .Include(c => c.CourseAllocation)
                .ThenInclude(a => a.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(s => s.SessionSemester)
                .Include(u => u.SubInstructor)
                .ThenInclude(p => p.Person)
                .ToListAsync();
            if (subInstructors != null)
            {
                foreach (var item in subInstructors)
                {
                    GetSubInstructorDto subInstructorDto = new GetSubInstructorDto();
                    subInstructorDto.CourseId = item.CourseAllocation.CourseId;
                    subInstructorDto.CourseName = item.CourseAllocation.Course.CourseTitle;
                    subInstructorDto.UserId = item.SubInstructorId;
                    subInstructorDto.PersonName = item.SubInstructor.Person.Surname + " " + item.SubInstructor.Person.Firstname + " " + item.SubInstructor.Person.Othername;
                    subInstructorDto.DateAdded = item.DateCreated;
                    subInstructorDto.Id = item.Id;
                    subinstructors_list.Add(subInstructorDto);
                }
            }

            return subinstructors_list;

        }

        public async Task<IEnumerable<SubInstructorCourseDto>> GetSubInstructorCourses(long SubInstructorId)
        {
            return await _context.COURSE_SUB_ALLOCATION.Where(s => s.SubInstructorId == SubInstructorId && s.CourseAllocation.SessionSemester.Active)
                .Include(c => c.CourseAllocation)
                .ThenInclude(a => a.Course)
                .Include(c => c.CourseAllocation)
                .ThenInclude(s => s.SessionSemester)
                .Select(f => new SubInstructorCourseDto
                {
                    CourseName = f.CourseAllocation.Course.CourseTitle,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseId = f.CourseAllocation.CourseId,
                    Id = f.Id
                })
                .ToListAsync();
        }
        public async Task<ResponseModel> DeleteSubInstructor(long Id)
        {
            ResponseModel response = new ResponseModel();

            if (Id > 0)
            {
                var getInstructor = await _context.COURSE_SUB_ALLOCATION.Where(p => p.Id == Id).FirstOrDefaultAsync();
                _context.Remove(getInstructor);
                await _context.SaveChangesAsync();
                response.Message = "Success";
                response.StatusCode = 200;
            }
            return response;
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
