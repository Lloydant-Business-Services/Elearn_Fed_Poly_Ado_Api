using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICourseService
    {
        Task<ResponseModel> AddCourse(AddCourseDto courseDto);
        Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto);
        Task<IEnumerable<AddCourseDto>> GetStudentCourses();
        Task<IEnumerable<AllCoursesDto>> GetCourses();
        Task<IEnumerable<GetDepartmentCourseDto>> GetDepartmentalCourses(long departmentId);
        Task<AllCoursesDto> GetCourseByCourseId(long courseId);
        Task<AllCoursesDto> AllocatedCourseByAllocationId(long courseAllocationId);
        Task<IEnumerable<GetDepartmentCourseDto>> GetAllocatedCoursesByDepartment(long departmentId);
        Task<CoursePloadSheetAggregation> ProcessBulkCourseUpload(IEnumerable<CourseUploadModel> courseList, long userId);
        Task<IEnumerable<AddCourseDto>> GetStudentCoursesByDepartment(long departmentId);
        Task<IEnumerable<GetDepartmentCourseDto>> GetAllocatedServiceCourses();
        //Task<ResponseModel> AllocateCourse(AllocateCourseDto dto);
    }
}
