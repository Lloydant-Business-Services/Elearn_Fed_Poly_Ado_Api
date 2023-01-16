using APIs.Middleware;
using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    //[AuthorizeRole(ElearnRole.SCHOOLADMIN, ElearnRole.INSTRUCTOR, ElearnRole.HOD)]
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorHodController : ControllerBase
    {
        private readonly IInstructorHodService _service;
        public InstructorHodController(IInstructorHodService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructors() => await _service.GetInstututionInstructors();
        [HttpPost("[action]")]
        public async Task<ResponseModel> AddCourseInstructorAndHod(AddUserDto userDto) => await _service.AddCourseInstructorAndHod(userDto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByDepartmentId(long departmentId) => await _service.GetInstructorsByDepartmentId(departmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetAllInstructors(long departmentId) => await _service.GetAllInstructors();
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetAllDepartmentHeads() => await _service.GetAllDepartmentHeads();
        [HttpGet("[action]")]
        public async Task<IEnumerable<InstructorCoursesDto>> GetInstructorCoursesByUserId(long instructorUserId) => await _service.GetInstructorCoursesByUserId(instructorUserId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstructorsByFacultyId(long facultyId) => await _service.GetInstructorsByFacultyId(facultyId);
        [HttpGet("[action]")]
        public async Task<HODDashboardSummaryDto> HODDashboardSummary(long DepartmentId) => await _service.HODDashboardSummary(DepartmentId);
        [HttpGet("[action]")]
        public async Task<InstructorSummaryDto> InstructorDashboardSummary(long InstructorId) => await _service.InstructorDashboardSummary(InstructorId);
        [HttpPost("[action]")]
        public async Task<bool> RemoveHod(long DepartmentId) => await _service.RemoveHod(DepartmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstututionInstructorsAndHodPerson(string searchInput)
        {
            return await _service.GetInstututionInstructorsAndHodPerson(searchInput);
        }
        [HttpPost("[action]")]
        public async Task<bool> RemoveInstructor(long userId) => await _service.RemoveInstructor(userId);
    }
}
