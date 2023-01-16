using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataLayer.Dtos.AddSubInstructorDto;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubInstructorController : ControllerBase
    {
        private readonly ISubInstructorService _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public SubInstructorController(ISubInstructorService service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }

        [HttpPost("[action]")]
        public async Task<ResponseModel> AddSubInstructor(AddSubInstructorDto addSubInstructorDto) => await _service.AddSubInstructor(addSubInstructorDto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetSubInstructorDto>> GetCourseSubIntructors(long courseId) => await _service.GetCourseSubIntructors(courseId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<SubInstructorCourseDto>> GetSubInstructorCourses(long SubInstructorId) => await _service.GetSubInstructorCourses(SubInstructorId);
        [HttpPost("[action]")]
        public async Task<ResponseModel> DeleteSubInstructor(long Id) => await _service.DeleteSubInstructor(Id);
    }

}
