using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignment_Service _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public AssignmentController(IAssignment_Service service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }


        [HttpPost("CreateAssignment")]
        public async Task<AssignmentDto> PostAssignment([FromForm] AddAssignmentDto addAssignmentDto)
        {
            var directory = Path.Combine("Resources", "Assignment");
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
            return await _service.AddAssignment(addAssignmentDto, filePath, directory);
        }


        [HttpGet("[action]")]
        public async Task<AssignmentDto> GetAssignmentByAssignmentId(long AssignmentId) => await _service.GetAssignmentByAssignmentId(AssignmentId);

        [HttpPost("SubmitStudentAssignment")]
        public async Task<ResponseModel> PostStudentAssignment([FromForm] StudentAssignmentSubmissionDto studentAssignmentSubmissionDto)
        {
            var directory = Path.Combine("Resources", "AssignmentSubmission");
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
            return await _service.AddAssignmentSubmission(studentAssignmentSubmissionDto, filePath, directory);
        }
        [HttpPost("[action]")]
        public async Task<AssignmentSubmissionDto> GradeAssignment(GradeAssignmentDto gradeAssignmentDto) => await _service.GradeAssignment(gradeAssignmentDto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId) => await _service.ListAssignmentByCourseId(courseId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByInstructorUserId(long userId) => await _service.ListAssignmentByInstructorUserId(userId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByStudentId(long StudentUserId) => await _service.ListAssignmentByStudentId(StudentUserId);
        [HttpPost("[action]")]
        public async Task PublishResultAssignment(AssignmentPublishDto assignmentPublishDto) => await _service.PublishResultAssignment(assignmentPublishDto);
        [HttpPost("[action]")]
        public async Task DeleteAssignment(DeleteRecordDto deleteRecordDto) => await _service.DeleteAssignment(deleteRecordDto);
        [HttpGet("[action]")]
        public async Task<int> AssignmentCountBy(long UserId) => await _service.AssignmentCountBy(UserId);
        [HttpPost("[action]")]
        public async Task<AssignmentDto> EditAssignment(UpdateAssignmentDto updateAssignmentDto) => await _service.EditAssignment(updateAssignmentDto);
        [HttpGet("[action]")]
        public async Task<AssignmentSubmissionDto> GetAssignmentSubmissionById(long AssignmentSubmissionId) => await _service.GetAssignmentSubmissionById(AssignmentSubmissionId);
        [HttpGet("[action]")]
        public async Task<StudentPersonDetailCountDto> StudentPersonStats(long PersonId) => await _service.StudentPersonStats(PersonId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<AssignmentSubmissionDto>> GetAllAssignmentSubmissionByAssignemntId(long AssignmentId) => await _service.GetAllAssignmentSubmissionByAssignemntId(AssignmentId);

        [HttpGet("[action]")]
        public async Task<AssignmentSubmissionDto> GetAssignmentSubmissionBy(long AssignmentId, long StudentUserId) => await _service.GetAssignmentSubmissionBy(AssignmentId, StudentUserId);
        [HttpPost("[action]")]
        public async Task<ResponseModel> ExtendAssignmentDueDate(AssignmentDueDateDto dto) => await _service.ExtendAssignmentDueDate(dto);
    }
}
