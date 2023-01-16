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
    public class CourseMaterialController : ControllerBase
    {
        private readonly ICourseMaterialService _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public CourseMaterialController(ICourseMaterialService service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId) => await _service.ListAssignmentByCourseId(courseId);
        [HttpPost("[action]")]
        public async Task<long> CreateCourseTopic(AddCourseTopicDto addCourseTopicDto) => await _service.CreateCourseTopic(addCourseTopicDto);
        [HttpGet("GetCourseTopicByCourseAllocaionId")]
        public async Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicBy(long CourseAllocationId) => await _service.GetCourseTopicBy(CourseAllocationId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicByInstructor(long userId) => await _service.GetCourseTopicByInstructor(userId);
        [HttpPost("CreateCourseContent")]
        public async Task<long> PostCourseContent([FromForm] AddCourseContentDto addCourseContentDto)
        {
            var directory = Path.Combine("Resources", "CourseContentNote");
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
            return await _service.AddCourseContent(addCourseContentDto, filePath, directory);
        }
        [HttpGet("GetContentByTopic")]
        public async Task<IEnumerable<GetCourseContentDto>> GetContentBy(long TopicId) => await _service.GetContentBy(TopicId);
        [HttpGet("GetContentByInstructor")]
        public async Task<IEnumerable<GetCourseContentDto>> GetAllContentUserId(long userId) => await _service.GetAllContentUserId(userId);
        [HttpPost("[action]")]
        public async Task<bool> DeleteCourseContent(long courseContentId) => await _service.DeleteCourseContent(courseContentId);
        [HttpPost("[action]")]
        public async Task<bool> DeleteCourseTopic(long TopicId) => await _service.DeleteCourseTopic(TopicId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByInstructorId(long InstructorId) => await _service.GetCourseMaterialByInstructorId(InstructorId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByCourseId(long CourseId) => await _service.GetCourseMaterialByCourseId(CourseId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByDepartmentId(long DepartmentId) => await _service.GetCourseMaterialByDepartmentId(DepartmentId);

        [HttpPost("[action]")]
        public async Task<bool> EditCourseTopic(long TopicId, AddCourseTopicDto dto) => await _service.EditCourseTopic(TopicId, dto);
        [HttpPost("[action]")]
        public async Task<bool> ImportCourseMaterialByTopic(long contentId, long newTopicId) => await _service.ImportCourseMaterialByTopic(contentId, newTopicId);
    }
}

