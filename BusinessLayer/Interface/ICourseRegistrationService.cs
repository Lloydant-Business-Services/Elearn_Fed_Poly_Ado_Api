using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICourseRegistrationService
    {
        Task<ResponseModel> RegisterCourses(RegisterCourseDto dto);
        Task<IEnumerable<GetRegisteredCoursesDto>> GetRegisteredCourses(long personId, long sessionSemesterId);
        Task<ResponseModel> RegisterCourseSingle(RegisterCourseSingleDto dto);
    }
}
