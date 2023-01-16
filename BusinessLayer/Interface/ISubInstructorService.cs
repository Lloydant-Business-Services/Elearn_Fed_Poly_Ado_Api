using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DataLayer.Dtos.AddSubInstructorDto;

namespace BusinessLayer.Interface
{
    public interface ISubInstructorService
    {
        Task<ResponseModel> AddSubInstructor(AddSubInstructorDto addSubInstructorDto);
        Task<IEnumerable<GetSubInstructorDto>> GetCourseSubIntructors(long courseId);
        Task<IEnumerable<SubInstructorCourseDto>> GetSubInstructorCourses(long SubInstructorId);
        Task<ResponseModel> DeleteSubInstructor(long Id);
    }
}
