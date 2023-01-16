using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseRegistrationController : ControllerBase
    {
        private readonly ICourseRegistrationService _service;
        public CourseRegistrationController(ICourseRegistrationService service)
        {
            _service = service;
        }
        [HttpPost("RegisterCoursesBulk")]
        public async Task<ResponseModel> RegisterCourses(RegisterCourseDto dto) => await _service.RegisterCourses(dto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetRegisteredCoursesDto>> GetRegisteredCourses(long personId, long sessionSemesterId) => await _service.GetRegisteredCourses(personId, sessionSemesterId);

        [HttpPost("[action]")]
        public async Task<ResponseModel> RegisterCourseSingle(RegisterCourseSingleDto dto) => await _service.RegisterCourseSingle(dto);
    }
}
