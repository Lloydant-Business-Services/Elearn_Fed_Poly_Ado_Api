using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IQuizService
    {
        Task<QuizDto> AddQuiz(AddQuizDto addQuizDto, string filePath, string directory);
        Task<QuizDto> GetQuizByQuizId(long QuizId);
        Task<ResponseModel> AddQuizSubmission(StudentQuizSubmissionDto studentQuizSubmissionDto, string filePath, string directory);
        Task<QuizSubmissionDto> GradeQuiz(GradeQuizDto gradeQuizDto);
        Task<IEnumerable<QuizListDto>> ListQuizByCourseId(long courseId);
        Task<IEnumerable<QuizListDto>> ListQuizByInstructorUserId(long userId);
        Task<IEnumerable<QuizListDto>> ListQuizByStudentId(long StudentUserId);
        Task PublishResultQuiz(QuizPublishDto QuizPublishDto);
        Task DeleteQuiz(DeleteRecordDto deleteRecordDto);
        Task<int> QuizCountBy(long UserId);
        Task<QuizDto> EditQuiz(UpdateQuizDto updateQuizDto);
        Task<QuizSubmissionDto> GetQuizSubmissionById(long QuizSubmissionId);
        //Task<StudentPersonDetailCountDto> StudentPersonStats(long PersonId);
        Task<IEnumerable<QuizSubmissionDto>> GetAllQuizSubmissionByAssignemntId(long QuizId);
        Task<QuizSubmissionDto> GetQuizSubmissionBy(long QuizId, long StudentUserId);
        Task<ResponseModel> ExtendQuizDueDate(QuizDueDateDto dto);
    }
}
