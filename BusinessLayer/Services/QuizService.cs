using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class QuizService : IQuizService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string baseUrl;
        //private readonly IFileUpload _fileUpload;

        public QuizService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
            //_fileUpload = fileUpload;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        public async Task<QuizDto> AddQuiz(AddQuizDto addQuizDto, string filePath, string directory)
        {
            if (addQuizDto == null)
                return null;
            if (addQuizDto.CourseAllocationId == 0)
                throw new NullReferenceException("No Course Id");
            var course = await _context.COURSE_ALLOCATION.Where(f => f.Id == addQuizDto.CourseAllocationId && f.Active)
                .Include(c => c.Course)
                .FirstOrDefaultAsync();
            if (course == null)
                throw new NullReferenceException("Course not allocated");
            var saveQuizLink = string.Empty;
            if (addQuizDto.QuizUpload != null)
            {

                addQuizDto.Name = addQuizDto.Name.Replace(" ", "");
                string fileNamePrefix = course.Course.CourseCode + "_" + addQuizDto.Name + "_" + DateTime.Now.Millisecond;
                saveQuizLink = await GetNoteUploadLink(addQuizDto.QuizUpload, filePath, directory, fileNamePrefix);
            }
            Quiz Quiz = new Quiz()
            {
                Active = true,
                DueDate = addQuizDto.DueDate,
                IsDelete = false,
                SetDate = DateTime.Now,
                QuizInstruction = addQuizDto.QuizInstruction,
                QuizInText = addQuizDto.QuizInText,
                QuizName = addQuizDto.Name,
                QuizUploadLink = saveQuizLink,
                QuizVideoLink = addQuizDto.QuizVideoLink,
                CourseAllocation = course,
                MaxScore = addQuizDto.MaxScore,
                PublishResult = false,
                CharacterLimit = addQuizDto.MaxCharacters
            };
            _context.Add(Quiz);
            var isSuccessfull = await _context.SaveChangesAsync();
            if (isSuccessfull > 0)
            {
                return await GetQuizByQuizId(Quiz.Id);
            }
            return null;

        }

        public async Task<ResponseModel> AddQuizSubmission(StudentQuizSubmissionDto studentQuizSubmissionDto, string filePath, string directory)
        {
            ResponseModel response = new ResponseModel();
            if (studentQuizSubmissionDto == null)
                return null;
            if (studentQuizSubmissionDto?.QuizId == 0)
                throw new NullReferenceException("No Quiz Id");
            if (studentQuizSubmissionDto?.StudentUserId == 0)
                throw new NullReferenceException("No StudentUser Id");
            var Quiz = await _context.QUIZ
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Where(f => f.Id == studentQuizSubmissionDto.QuizId).FirstOrDefaultAsync();
            if (Quiz == null)
                throw new NullReferenceException("Quiz does not exist");
            var user = await _context.USER
                .Include(f => f.Person)
                .Where(f => f.Id == studentQuizSubmissionDto.StudentUserId).FirstOrDefaultAsync();
            if (user == null)
                throw new NullReferenceException("Student does not exist");
            if (Quiz.DueDate <= DateTime.Now.AddHours(8))
                {
                    response.Message = "Failed to submit! Time allowed for the exercise has past";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }
            var courseRegistration = await _context.COURSE_REGISTRATION.Where(f => f.CourseAllocation.CourseId == Quiz.CourseAllocation.CourseId && f.StudentPerson.PersonId == user.PersonId).FirstOrDefaultAsync();

            var isSubmitted = await _context.QUIZ_SUBMISSION.Where(a => a.QuizId == studentQuizSubmissionDto.QuizId && a.CourseRegistrationId == courseRegistration.Id).FirstOrDefaultAsync();
            if (isSubmitted != null)
            {
                response.Message = "Error! Quiz Aready Submitted";
                response.StatusCode = StatusCodes.Status208AlreadyReported;
                return response;
            }

            var saveQuizLink = string.Empty;
            if (studentQuizSubmissionDto.QuizUpload != null)
            {
                Quiz.CourseAllocation.Course.CourseCode = Quiz.CourseAllocation.Course.CourseCode.Replace(" ", "");
                string fileNamePrefix = user.Person.Surname + "_" + user.Person.Firstname + "_" + Quiz.CourseAllocation.Course.CourseCode + "_" + DateTime.Now.Millisecond;
                saveQuizLink = await GetNoteUploadLink(studentQuizSubmissionDto.QuizUpload, filePath, directory, fileNamePrefix);
            }

            if (courseRegistration == null)
                throw new NullReferenceException("Student did not register this course");
            QuizSubmission QuizSubmission = new QuizSubmission()
            {
                Active = true,
                Quiz = Quiz,
                DateSubmitted = DateTime.Now,
                QuizInTextSubmission = studentQuizSubmissionDto.QuizInText,
                QuizSubmissionHostedLink = studentQuizSubmissionDto.QuizHostedLink,
                QuizSubmissionUploadLink = saveQuizLink,
                CourseRegistration = courseRegistration,
            };
            _context.Add(QuizSubmission);
            var isSuccessful = await _context.SaveChangesAsync();
            if (isSuccessful > 0)
            {
                return response;

            }
            return null;
        }

        public async Task<QuizSubmissionDto> GradeQuiz(GradeQuizDto gradeQuizDto)
        {
            if (gradeQuizDto == null)
                throw new NullReferenceException("No Content");
            if (gradeQuizDto?.QuizSubmissionId == 0)
                throw new NullReferenceException("No Quiz Submission Id");
            var QuizSubmission = await _context.QUIZ_SUBMISSION.Where(f => f.Id == gradeQuizDto.QuizSubmissionId).FirstOrDefaultAsync();
            if (QuizSubmission == null)
                throw new NullReferenceException("No Quiz Submission Record");
            QuizSubmission.Score = gradeQuizDto.Score;
            QuizSubmission.InstructorRemark = gradeQuizDto.Remark;
            _context.Update(QuizSubmission);
            await _context.SaveChangesAsync();
            return await GetQuizSubmissionById(QuizSubmission.Id);

        }

        public async Task<IEnumerable<QuizListDto>> ListQuizByCourseId(long courseId)
        {
            if (courseId == 0)
                throw new NullReferenceException("No Course Id");
            return await _context.QUIZ.Where(f => f.CourseAllocation.CourseId == courseId && !f.IsDelete)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new QuizListDto
                {
                    QuizId = f.Id,
                    Active = f.Active,
                    QuizName = f.QuizName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore,
                    MaxCharacters = f.CharacterLimit
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizListDto>> ListQuizByInstructorUserId(long userId)
        {
            if (userId == 0)
                throw new NullReferenceException("No Course Id");
            return await _context.QUIZ
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Where(f => f.CourseAllocation.Instructor.Id == userId && f.CourseAllocation.SessionSemester.Active && !f.IsDelete)
                .Select(f => new QuizListDto
                {
                    QuizId = f.Id,
                    Active = f.Active,
                    QuizName = f.QuizName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore,
                    MaxCharacters = f.CharacterLimit
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<QuizListDto>> ListQuizByStudentId(long StudentUserId)
        {
            List<QuizListDto> allAssgnment = new List<QuizListDto>();
            if (StudentUserId == 0)
                throw new NullReferenceException("No Student-User Id");
            var user = await _context.USER
                .Where(f => f.Id == StudentUserId).FirstOrDefaultAsync();
            if (user == null)
                throw new NullReferenceException("Student don't Exist");
            //get the active sessionSemester of the students institutiuon
            var institutionActiveSessionSemester = await GetActiveSessionSemester();
            if (institutionActiveSessionSemester == null)
                throw new NullReferenceException("Your Institution does not have any active session semester");
            //get the course registered by the student in the active session semester
            var courseRegistration = await _context.COURSE_REGISTRATION
                .Include(f => f.StudentPerson)
                .ThenInclude(f => f.Person)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Where(f => f.Active && f.StudentPerson.PersonId == user.PersonId && f.CourseAllocation.SessionSemesterId == institutionActiveSessionSemester.Id).ToListAsync();
            if (institutionActiveSessionSemester == null)
                return null;
            //get all the Quiz for the student based on the registered course
            foreach (var item in courseRegistration)
            {
                var submssionStatus = await _context.QUIZ_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id).FirstOrDefaultAsync();
                var Quizs = await _context.QUIZ
               .Include(f => f.CourseAllocation)
               .ThenInclude(c => c.Course)
               .Include(f => f.CourseAllocation)
               .ThenInclude(f => f.Instructor)
               .ThenInclude(f => f.Person)
               .Where(f => f.CourseAllocationId == item.CourseAllocationId && !f.IsDelete)
               .Select(f => new QuizListDto
               {
                   QuizId = f.Id,
                   Active = f.Active,
                   QuizName = f.QuizName,
                   CourseCode = f.CourseAllocation.Course.CourseCode,
                   CourseTitle = f.CourseAllocation.Course.CourseTitle,
                   DueDate = f.DueDate,
                   InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                   IsPublished = f.PublishResult,
                   MaxScore = f.MaxScore,
                   IsSubmitted = submssionStatus != null ? true : false
               })
               .ToListAsync();
                allAssgnment.AddRange(Quizs);
            }
            return allAssgnment;
        }
        public async Task PublishResultQuiz(QuizPublishDto QuizPublishDto)
        {
            if (QuizPublishDto == null)
                throw new NullReferenceException("Please supply arguments");
            if (QuizPublishDto?.QuizId == 0)
                throw new NullReferenceException("Please supply arguments");
            var Quiz = await _context.QUIZ.Where(f => f.Id == QuizPublishDto.QuizId && !f.IsDelete).FirstOrDefaultAsync();
            if (Quiz == null)
                throw new NullReferenceException("Quiz not found");
            Quiz.PublishResult = QuizPublishDto.Publish;
            _context.Update(Quiz);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteQuiz(DeleteRecordDto deleteRecordDto)
        {
            if (deleteRecordDto == null)
                throw new NullReferenceException("Please supply arguments");
            if (deleteRecordDto?.RecordId == 0)
                throw new NullReferenceException("Please supply arguments");
            var Quiz = await _context.QUIZ.Where(f => f.Id == deleteRecordDto.RecordId).FirstOrDefaultAsync();
            if (Quiz == null)
                throw new NullReferenceException("Quiz not found");
            Quiz.IsDelete = true;
            _context.Update(Quiz);
            await _context.SaveChangesAsync();
        }

        public async Task<int> QuizCountBy(long UserId)
        {
            if (UserId == 0)
                throw new NullReferenceException("No User Id");
            return await _context.QUIZ
                 .Include(f => f.CourseAllocation)
                 .ThenInclude(c => c.Course)
                 .Include(f => f.CourseAllocation)
                 .ThenInclude(f => f.Instructor)
                 .ThenInclude(f => f.Person)
                 .Where(f => f.CourseAllocation.InstructorId == UserId && f.CourseAllocation.SessionSemester.Active && !f.IsDelete).CountAsync();

        }
     
        public async Task<QuizDto> EditQuiz(UpdateQuizDto updateQuizDto)
        {
            if (updateQuizDto == null)
                return null;
            if (updateQuizDto.QuizId == 0)
                throw new NullReferenceException("No Quiz Id");
            var Quiz = await _context.QUIZ.Where(f => f.Id == updateQuizDto.QuizId && !f.IsDelete).FirstOrDefaultAsync();
            if (Quiz == null)
                throw new NullReferenceException("Quiz does not exist");
            Quiz.MaxScore = updateQuizDto.MaxScore;
            Quiz.SetDate = updateQuizDto.SetDate;
            Quiz.DueDate = updateQuizDto.DueDate;
            Quiz.QuizInstruction = updateQuizDto.QuizInstruction;
            Quiz.QuizInText = updateQuizDto.QuizInText;
            Quiz.QuizName = updateQuizDto.QuizName;
            _context.Update(Quiz);
            var isSuccessfull = await _context.SaveChangesAsync();
            if (isSuccessfull > 0)
            {
                return await GetQuizByQuizId(Quiz.Id);
            }
            return null;
        }
        public async Task<QuizSubmissionDto> GetQuizSubmissionById(long QuizSubmissionId)
        {
            if (QuizSubmissionId == 0)
                throw new NullReferenceException("No Quiz Submission Id");
            //var isPublished = await _context
            return await _context.QUIZ_SUBMISSION
                 .Include(f => f.Quiz)
                 .ThenInclude(f => f.CourseAllocation.Course)
                 .ThenInclude(f => f.User)
                 .ThenInclude(f => f.Person)
                 .Include(f => f.CourseRegistration)
                 .ThenInclude(f => f.StudentPerson)
                 .ThenInclude(f => f.Person)
                 .Where(f => f.Id == QuizSubmissionId)
                 .Select(f => new QuizSubmissionDto
                 {
                     Active = f.Active,
                     DateSubmitted = f.DateSubmitted,
                     QuizInTextSubmission = f.QuizInTextSubmission,
                     QuizSubmissionId = f.Id,
                     QuizSubmissionUploadLink = f.QuizSubmissionUploadLink != null ? baseUrl + f.QuizSubmissionUploadLink : null,
                     InstructorRemark = f.InstructorRemark,
                     Score = f.Score,
                     StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                     MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                     QuizSubmissionHostedLink = f.QuizSubmissionHostedLink,
                     IsPublished = f.Quiz.PublishResult,
                     QuizId = f.Quiz.Id

                 })
                 .FirstOrDefaultAsync();
        }
        public async Task<QuizDto> GetQuizByQuizId(long QuizId)
        {
            if (QuizId == 0)
                throw new NullReferenceException("No Quiz Id");
            return await _context.QUIZ.Where(f => f.Id == QuizId)
                .Include(f => f.CourseAllocation.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new QuizDto
                {
                    QuizId = f.Id,
                    Active = f.Active,
                    QuizName = f.QuizName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore,
                    QuizInstruction = f.QuizInstruction,
                    QuizInText = f.QuizInText,
                    QuizUploadLink = f.QuizUploadLink != null ?  baseUrl + f.QuizUploadLink : null,
                    IsDeleted = f.IsDelete,
                    QuizVideoLink = f.QuizVideoLink,
                    SetDate = f.SetDate,

                })
                .FirstOrDefaultAsync();
        }
        public async Task<GetSessionSemesterDto> GetActiveSessionSemester()
        {
            return await _context.SESSION_SEMESTER.Where(a => a.Active)
                .Include(s => s.Semester)
                .Include(s => s.Session)
                .Select(f => new GetSessionSemesterDto
                {
                    SemesterName = f.Semester.Name,
                    SessionName = f.Session.Name,
                    SemesterId = f.SemesterId,
                    SessionId = f.SessionId,
                    Id = f.Id
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetNoteUploadLink(IFormFile file, string filePath, string directory, string givenFileName)
        {

            var noteUrl = string.Empty;
            //Define allowed property of the uploaded file

            var validFileSize = (1024 * 1024);//1mb
            List<string> validFileExtension = new List<string>();
            validFileExtension.Add(".pdf");
            validFileExtension.Add(".doc");
            validFileExtension.Add(".docx");
            validFileExtension.Add(".xlx");
            validFileExtension.Add(".xlxs");
            validFileExtension.Add(".docx");
            validFileExtension.Add(".ppt");
            if (file.Length > 0)
            {

                var extType = Path.GetExtension(file.FileName);
                var fileSize = file.Length;
                if (fileSize <= validFileSize)
                {

                    if (validFileExtension.Contains(extType))
                    {
                        string fileName = string.Format("{0}{1}", givenFileName + "_" + DateTime.Now.Millisecond, extType);
                        //create file path if it doesnt exist
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fullPath = Path.Combine(filePath, fileName);
                        noteUrl = Path.Combine(directory, fileName);
                        //Delete if file exist
                        FileInfo fileExists = new FileInfo(fullPath);
                        if (fileExists.Exists)
                        {
                            fileExists.Delete();
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return noteUrl = noteUrl.Replace('\\', '/');





                    }
                    else
                    {
                        throw new BadImageFormatException("File format is not supported");
                    }
                }
            }
            return noteUrl;
        }
        public async Task<IEnumerable<QuizSubmissionDto>> GetAllQuizSubmissionByAssignemntId(long QuizId)
        {
            if (QuizId == 0)
                throw new NullReferenceException("No Quiz Id");
            //var isPublished = await _context
            return await _context.QUIZ_SUBMISSION
                 .Include(f => f.Quiz)
                 .ThenInclude(f => f.CourseAllocation.Course)
                 .ThenInclude(f => f.User)
                 .ThenInclude(f => f.Person)
                 .Include(f => f.CourseRegistration)
                 .ThenInclude(f => f.StudentPerson)
                 .ThenInclude(f => f.Person)
                 .Where(f => f.QuizId == QuizId)
                 .Select(f => new QuizSubmissionDto
                 {
                     Active = f.Active,
                     DateSubmitted = f.DateSubmitted,
                     QuizInTextSubmission = f.QuizInTextSubmission,
                     QuizSubmissionId = f.Id,
                     QuizSubmissionUploadLink = f.QuizSubmissionUploadLink != null ? baseUrl + f.QuizSubmissionUploadLink : null,
                     InstructorRemark = f.InstructorRemark,
                     Score = f.Score,
                     StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                     MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                     QuizSubmissionHostedLink = f.QuizSubmissionHostedLink,
                     IsPublished = f.Quiz.PublishResult,
                     QuizId = f.Quiz.Id
                 })
                 .ToListAsync();
        }

        public async Task<QuizSubmissionDto> GetQuizSubmissionBy(long QuizId, long StudentUserId)
        {
            try
            {
                if (QuizId == 0 || StudentUserId == 0)
                    throw new NullReferenceException("No Quiz Submission Id");
                var getUser = await _context.USER.Where(i => i.Id == StudentUserId).Include(p => p.Person).FirstOrDefaultAsync();

                //var isPublished = await _context
                return await _context.QUIZ_SUBMISSION
                     .Include(f => f.Quiz)
                     .ThenInclude(f => f.CourseAllocation.Course)
                     .ThenInclude(f => f.User)
                     .ThenInclude(f => f.Person)
                     .Include(f => f.CourseRegistration)
                     .ThenInclude(f => f.StudentPerson)
                     .ThenInclude(f => f.Person)
                     .Where(f => f.QuizId == QuizId && f.CourseRegistration.StudentPerson.Person.Id == getUser.Person.Id)
                     .Select(f => new QuizSubmissionDto
                     {
                         Active = f.Active,
                         DateSubmitted = f.DateSubmitted,
                         QuizInTextSubmission = f.QuizInTextSubmission,
                         QuizSubmissionId = f.Id,
                         QuizSubmissionUploadLink = f.QuizSubmissionUploadLink != null ? baseUrl + f.QuizSubmissionUploadLink : null,
                         InstructorRemark = f.InstructorRemark,
                         Score = f.Score,
                         StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                         MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                         QuizSubmissionHostedLink = f.QuizSubmissionHostedLink,
                         IsPublished = f.Quiz.PublishResult,
                         QuizId = f.Quiz.Id
                     })
                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<ResponseModel> ExtendQuizDueDate(QuizDueDateDto dto)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var Quiz = await _context.QUIZ.Where(x => x.Id == dto.QuizId).FirstOrDefaultAsync();
                if (Quiz != null)
                {
                    Quiz.DueDate = dto.DueDate;
                    _context.Update(Quiz);
                    await _context.SaveChangesAsync();
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }
        }
    }
}
