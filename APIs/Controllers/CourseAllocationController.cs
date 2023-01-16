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
    public class CourseAllocationController : ControllerBase
    {
        private readonly ICourseAllocationService _service;
        public CourseAllocationController(ICourseAllocationService service)
        {
            _service = service;
        }

        [HttpPost("AllocateCourse")]
        public async Task<ResponseModel> AllocateCourse(AllocateCourseDto dto) => await _service.AllocateCourse(dto);
        [HttpPost("ClearAllocation")]
        public async Task<int> ClearCourseAllocation() => await _service.ClearCourseAllocation();
        //[HttpGet("GetAllInstructors")]
        //public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors() => await _service.GetInstututionInstructors();
        //[HttpPost("[action]")]
        //public async Task<long> AddCourseInstructorAndHod(AddUserDto userDto) => await _service.AddCourseInstructorAndHod(userDto);
        //[HttpGet("[action]")]
        //public async Task<IEnumerable<GetCourseInstructorDto>> GetInstructorsByDepartmentId(long departmentId) => await _service.GetInstructorsByDepartmentId(departmentId);
        //[HttpGet("[action]")]
        //public async Task<IEnumerable<GetCourseInstructorDto>> GetAllDepartmentHeads() => await _service.GetAllDepartmentHeads();
    }
}
