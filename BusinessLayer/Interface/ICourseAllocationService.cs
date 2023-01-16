using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICourseAllocationService
    {
        Task<ResponseModel> AllocateCourse(AllocateCourseDto dto);
        Task<int> ClearCourseAllocation();
        //Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors();
        //Task<long> AddCourseInstructorAndHod(AddUserDto userDto);
        //Task<IEnumerable<GetCourseInstructorDto>> GetInstructorsByDepartmentId(long departmentId);
        //Task<IEnumerable<GetCourseInstructorDto>> GetAllDepartmentHeads();

    }
}
