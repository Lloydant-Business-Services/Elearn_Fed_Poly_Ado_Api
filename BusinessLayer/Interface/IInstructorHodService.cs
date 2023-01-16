using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IInstructorHodService 
    {
        Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors();
        Task<ResponseModel> AddCourseInstructorAndHod(AddUserDto userDto);
        Task<IEnumerable<GetInstructorDto>> GetInstructorsByDepartmentId(long departmentId);
        Task<IEnumerable<GetInstructorDto>> GetAllDepartmentHeads();
        Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesByUserId(long instructorUserId);
        Task<IEnumerable<GetInstructorDto>> GetInstructorsByFacultyId(long facultyId);
        Task<HODDashboardSummaryDto> HODDashboardSummary(long DepartmentId);
        Task<InstructorSummaryDto> InstructorDashboardSummary(long InstructorId);
        Task<bool> RemoveHod(long DepartmentId);
        Task<IEnumerable<GetInstructorDto>> GetInstututionInstructorsAndHodPerson(string searchInput);
        Task<bool> RemoveInstructor(long userId);
        Task<IEnumerable<GetInstructorDto>> GetAllInstructors();
    }
}
