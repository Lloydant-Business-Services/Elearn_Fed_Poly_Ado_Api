using BusinessLayer.Interface;
using DataLayer.Dtos;
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
    public class CourseAssignmentController : ControllerBase
    {
        private readonly ICourseAssignment _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public CourseAssignmentController(ICourseAssignment service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }

       //[HttpPost("CreateAssignment")]
       // public async Task<AssignmentDto> PostAssignment([FromForm] AddAssignmentDto addAssignmentDto)
       // {
       //     var directory = Path.Combine("Resources", "Assignment");
       //     var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
       //     return await _service.AddAssignment(addAssignmentDto, filePath, directory);
       // }


       // [HttpPost("[action]")]
       // public async Task<AssignmentDto> GetAssignmentByAssignmentId(long AssignmentId) => await _service.GetAssignmentByAssignmentId(AssignmentId);

       // [HttpPost("SubmitStudentAssignment")]
       // public async Task<AssignmentSubmissionDto> PostStudentAssignment([FromForm] StudentAssignmentSubmissionDto studentAssignmentSubmissionDto)
       // {
       //     var directory = Path.Combine("Resources", "AssignmentSubmission");
       //     var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
       //     return await _service.AddAssignmentSubmission(studentAssignmentSubmissionDto, filePath, directory);
       // }
       // [HttpPost("[action]")]
       // public async Task<AssignmentSubmissionDto> GradeAssignment(GradeAssignmentDto gradeAssignmentDto) => await _service.GradeAssignment(gradeAssignmentDto);
        //[HttpGet("[action]")]
        //public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId) => await _service.ListAssignmentByCourseId(courseId);
        //[HttpGet("[action]")]
        //public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByInstructorUserId(long userId) => await _service.ListAssignmentByInstructorUserId(userId);
        //[HttpGet("[action]")]
        //public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByStudentId(long StudentUserId) => await _service.ListAssignmentByStudentId(StudentUserId);
        //[HttpPost("[action]")]
        //public async Task PublishResultAssignment(AssignmentPublishDto assignmentPublishDto) => await _service.PublishResultAssignment(assignmentPublishDto);
        //[HttpDelete("[action]")]
        //public async Task DeleteAssignment(DeleteRecordDto deleteRecordDto) => await _service.DeleteAssignment(deleteRecordDto);
        //[HttpGet("[action]")]
        //public async Task<int> AssignmentCountBy(long UserId) => await _service.AssignmentCountBy(UserId);
        //[HttpPut("[action]")]
        //public async Task<AssignmentDto> EditAssignment(UpdateAssignmentDto updateAssignmentDto) => await _service.EditAssignment(updateAssignmentDto);
        //[HttpGet("[action]")]
        //public async Task<AssignmentSubmissionDto> GetAssignmentSubmissionById(long AssignmentSubmissionId) => await _service.GetAssignmentSubmissionById(AssignmentSubmissionId);
    }
}
