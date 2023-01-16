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
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public QuizController(IQuizService service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }


        [HttpPost("CreateQuiz")]
        public async Task<QuizDto> PostQuiz([FromForm] AddQuizDto addQuizDto)
        {
            var directory = Path.Combine("Resources", "Quiz");
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
            return await _service.AddQuiz(addQuizDto, filePath, directory);
        }


        [HttpGet("[action]")]
        public async Task<QuizDto> GetQuizByQuizId(long QuizId) => await _service.GetQuizByQuizId(QuizId);

        [HttpPost("SubmitStudentQuiz")]
        public async Task<ResponseModel> PostStudentQuiz([FromForm] StudentQuizSubmissionDto studentQuizSubmissionDto)
        {
            var directory = Path.Combine("Resources", "QuizSubmission");
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
            return await _service.AddQuizSubmission(studentQuizSubmissionDto, filePath, directory);
        }
        [HttpPost("[action]")]
        public async Task<QuizSubmissionDto> GradeQuiz(GradeQuizDto gradeQuizDto) => await _service.GradeQuiz(gradeQuizDto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<QuizListDto>> ListQuizByCourseId(long courseId) => await _service.ListQuizByCourseId(courseId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<QuizListDto>> ListQuizByInstructorUserId(long userId) => await _service.ListQuizByInstructorUserId(userId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<QuizListDto>> ListQuizByStudentId(long StudentUserId) => await _service.ListQuizByStudentId(StudentUserId);
        [HttpPost("[action]")]
        public async Task PublishResultQuiz(QuizPublishDto QuizPublishDto) => await _service.PublishResultQuiz(QuizPublishDto);
        [HttpPost("[action]")]
        public async Task DeleteQuiz(DeleteRecordDto deleteRecordDto) => await _service.DeleteQuiz(deleteRecordDto);
        [HttpGet("[action]")]
        public async Task<int> QuizCountBy(long UserId) => await _service.QuizCountBy(UserId);
        [HttpPost("[action]")]
        public async Task<QuizDto> EditQuiz(UpdateQuizDto updateQuizDto) => await _service.EditQuiz(updateQuizDto);
        [HttpGet("[action]")]
        public async Task<QuizSubmissionDto> GetQuizSubmissionById(long QuizSubmissionId) => await _service.GetQuizSubmissionById(QuizSubmissionId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<QuizSubmissionDto>> GetAllQuizSubmissionByAssignemntId(long QuizId) => await _service.GetAllQuizSubmissionByAssignemntId(QuizId);

        [HttpGet("[action]")]
        public async Task<QuizSubmissionDto> GetQuizSubmissionBy(long QuizId, long StudentUserId) => await _service.GetQuizSubmissionBy(QuizId, StudentUserId);
        [HttpPost("[action]")]
        public async Task<ResponseModel> ExtendQuizDueDate(QuizDueDateDto dto) => await _service.ExtendQuizDueDate(dto);
    }
}
