using BusinessLayer.Interface;
using DataLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingSevice _service;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public ReportingController(IReportingSevice service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");

        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetInstructorDto>> GetInstructors(long departmentId, long sessionSemesterId) => await _service.GetInstructors(departmentId, sessionSemesterId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<StudentReportListDto>> GetStudentsBy(long DepartmentId, long SessionSemesterId) => await _service.GetStudentsBy(DepartmentId, SessionSemesterId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<AssignmentCumulativeDto>> AssignmentCumulativeScore(long courseId, long sessionSemesterId, long departmentId) => await _service.AssignmentCumulativeScore(courseId, sessionSemesterId, departmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<ComprehensiveAssignmentCumulativeDto>> ComprehensiveAssignmentReport(long courseId, long sessionSemesterId, long departmentId) => await _service.ComprehensiveAssignmentReport(courseId, sessionSemesterId, departmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<ComprehensiveAssignmentCumulativeDto>> ComprehensiveQuizReport(long courseId, long sessionSemesterId, long departmentId) => await _service.ComprehensiveQuizReport(courseId, sessionSemesterId, departmentId);

        [HttpGet("[action]")]
        public async Task<StudentCumulativeAssignmentModel> CumulativeComprehensiveAssignmentReportByStudent(long personId, long sessionSemesterId) => await _service.CumulativeComprehensiveAssignmentReportByStudent(personId, sessionSemesterId);
    }
}
