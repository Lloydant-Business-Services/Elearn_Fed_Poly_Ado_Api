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
    public class Assignment_Service : IAssignment_Service
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string baseUrl;
        //private readonly IFileUpload _fileUpload;

        public Assignment_Service(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
            //_fileUpload = fileUpload;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        public async Task<AssignmentDto> AddAssignment(AddAssignmentDto addAssignmentDto, string filePath, string directory)
        {
            if (addAssignmentDto == null)
                return null;
            if (addAssignmentDto.CourseAllocationId == 0)
                throw new NullReferenceException("No Course Id");
            var course = await _context.COURSE_ALLOCATION.Where(f => f.Id == addAssignmentDto.CourseAllocationId && f.Active)
                .Include(c => c.Course)
                .FirstOrDefaultAsync();
            if (course == null)
                throw new NullReferenceException("Course not allocated");
            var saveAssignmentLink = string.Empty;
            if (addAssignmentDto.AssignmentUpload != null)
            {

                addAssignmentDto.Name = addAssignmentDto.Name.Replace(" ", "");
                string fileNamePrefix = course.Course.CourseCode + "_" + addAssignmentDto.Name + "_" + DateTime.Now.Millisecond;
                saveAssignmentLink = await GetNoteUploadLink(addAssignmentDto.AssignmentUpload, filePath, directory, fileNamePrefix);
            }
            Assignment assignment = new Assignment()
            {
                Active = true,
                DueDate = addAssignmentDto.DueDate,
                IsDelete = false,
                SetDate = DateTime.Now,
                AssignmentInstruction = addAssignmentDto.AssignmentInstruction,
                AssignmentInText = addAssignmentDto.AssignmentInText,
                AssignmentName = addAssignmentDto.Name,
                AssignmentUploadLink = saveAssignmentLink,
                AssignmentVideoLink = addAssignmentDto.AssignmentVideoLink,
                CourseAllocation = course,
                MaxScore = addAssignmentDto.MaxScore,
                PublishResult = false,
                MaxCharacters = addAssignmentDto.MaxCharacters
            };
            _context.Add(assignment);
            var isSuccessfull = await _context.SaveChangesAsync();
            if (isSuccessfull > 0)
            {
                return await GetAssignmentByAssignmentId(assignment.Id);
            }
            return null;

        }
     
        public async Task<ResponseModel> AddAssignmentSubmission(StudentAssignmentSubmissionDto studentAssignmentSubmissionDto, string filePath, string directory)
        {
            ResponseModel response = new ResponseModel();
            if (studentAssignmentSubmissionDto == null)
                return null;
            if (studentAssignmentSubmissionDto?.AssignmentId == 0)
                throw new NullReferenceException("No Assignment Id");
            if (studentAssignmentSubmissionDto?.StudentUserId == 0)
                throw new NullReferenceException("No StudentUser Id");
            var assignment = await _context.ASSIGNMENT
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .Where(f => f.Id == studentAssignmentSubmissionDto.AssignmentId).FirstOrDefaultAsync();
            if (assignment == null)
                throw new NullReferenceException("Assignment does not exist");
            var user = await _context.USER
                .Include(f => f.Person)
                .Where(f => f.Id == studentAssignmentSubmissionDto.StudentUserId).FirstOrDefaultAsync();
            if (user == null)
                throw new NullReferenceException("Student does not exist");

            var courseRegistration = await _context.COURSE_REGISTRATION.Where(f => f.CourseAllocation.CourseId == assignment.CourseAllocation.CourseId && f.StudentPerson.PersonId == user.PersonId).FirstOrDefaultAsync();

            var isSubmitted = await _context.ASSIGNMENT_SUBMISSION.Where(a => a.AssignmentId == studentAssignmentSubmissionDto.AssignmentId && a.CourseRegistrationId == courseRegistration.Id).FirstOrDefaultAsync();
            if(isSubmitted != null)
            {
                response.Message = "Error! Assignment Aready Submitted";
                response.StatusCode = StatusCodes.Status208AlreadyReported;
                return response;
            }

            var saveAssignmentLink = string.Empty;
            if (studentAssignmentSubmissionDto.AssignmentUpload != null)
            {
                assignment.CourseAllocation.Course.CourseCode = assignment.CourseAllocation.Course.CourseCode.Replace(" ", "");
                string fileNamePrefix = user.Person.Surname + "_" + user.Person.Firstname + "_" + assignment.CourseAllocation.Course.CourseCode + "_" + DateTime.Now.Millisecond;
                saveAssignmentLink = await GetNoteUploadLink(studentAssignmentSubmissionDto.AssignmentUpload, filePath, directory, fileNamePrefix);
            }
        
            if (courseRegistration == null)
                throw new NullReferenceException("Student did not register this course");
            bool lateSubmission = DateTime.Now > assignment.DueDate ? true : false;
            AssignmentSubmission assignmentSubmission = new AssignmentSubmission()
            {
                Active = true,
                Assignment = assignment,
                DateSubmitted = DateTime.Now,
                AssignmentInTextSubmission = studentAssignmentSubmissionDto.AssignmentInText,
                AssignmentSubmissionHostedLink = studentAssignmentSubmissionDto.AssignmentHostedLink,
                AssignmentSubmissionUploadLink = saveAssignmentLink,
                CourseRegistration = courseRegistration,
                IsLateSubmission = lateSubmission
            };
            _context.Add(assignmentSubmission);
            var isSuccessful = await _context.SaveChangesAsync();
            if (isSuccessful > 0)
            {
                return response;

            }
            return null;
        }

        public async Task<AssignmentSubmissionDto> GradeAssignment(GradeAssignmentDto gradeAssignmentDto)
        {
            if (gradeAssignmentDto == null)
                throw new NullReferenceException("No Content");
            if (gradeAssignmentDto?.AssignmentSubmissionId == 0)
                throw new NullReferenceException("No Assignment Submission Id");
            var assignmentSubmission = await _context.ASSIGNMENT_SUBMISSION.Where(f => f.Id == gradeAssignmentDto.AssignmentSubmissionId)
                .Include(x => x.Assignment)
                .FirstOrDefaultAsync();
            if (assignmentSubmission == null)
                throw new NullReferenceException("No Assignment Submission Record");
            if(gradeAssignmentDto.Score > assignmentSubmission.Assignment.MaxScore)
                throw new NullReferenceException("Graded Score cannot be greater than max score alloted");
            assignmentSubmission.Score = gradeAssignmentDto.Score;
            assignmentSubmission.InstructorRemark = gradeAssignmentDto.Remark;
            _context.Update(assignmentSubmission);
            await _context.SaveChangesAsync();
            return await GetAssignmentSubmissionById(assignmentSubmission.Id);

        }

        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId)
        {
            if (courseId == 0)
                throw new NullReferenceException("No Course Id");
            return await _context.ASSIGNMENT.Where(f => f.CourseAllocation.CourseId == courseId && !f.IsDelete)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new AssignmentListDto
                {
                    AssignmentId = f.Id,
                    Active = f.Active,
                    AssignmentName = f.AssignmentName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore,
                    MaxCharacters = f.MaxCharacters

                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByInstructorUserId(long userId)
        {
            if (userId == 0)
                throw new NullReferenceException("No Course Id");
            return await _context.ASSIGNMENT
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Where(f => f.CourseAllocation.Instructor.Id == userId && f.CourseAllocation.SessionSemester.Active && !f.IsDelete)
                .Select(f => new AssignmentListDto
                {
                    AssignmentId = f.Id,
                    Active = f.Active,
                    AssignmentName = f.AssignmentName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore,
                    MaxCharacters = f.MaxCharacters
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AssignmentListDto>> ListAssignmentByStudentId(long StudentUserId)
        {
            List<AssignmentListDto> allAssgnment = new List<AssignmentListDto>();
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
            //get all the assignment for the student based on the registered course
            foreach (var item in courseRegistration)
            {
                var submssionStatus = await _context.ASSIGNMENT_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id).FirstOrDefaultAsync();
                var assignments = await _context.ASSIGNMENT
               .Include(f => f.CourseAllocation)
               .ThenInclude(c => c.Course)
               .Include(f => f.CourseAllocation)
               .ThenInclude(f => f.Instructor)
               .ThenInclude(f => f.Person)
               .Where(f => f.CourseAllocationId == item.CourseAllocationId && !f.IsDelete)
               .Select(f => new AssignmentListDto
               {
                   AssignmentId = f.Id,
                   Active = f.Active,
                   AssignmentName = f.AssignmentName,
                   CourseCode = f.CourseAllocation.Course.CourseCode,
                   CourseTitle = f.CourseAllocation.Course.CourseTitle,
                   DueDate = f.DueDate,
                   InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                   IsPublished = f.PublishResult,
                   MaxScore = f.MaxScore,
                   IsSubmitted = submssionStatus != null ? true : false,
                   MaxCharacters = f.MaxCharacters
               })
               .ToListAsync();
                allAssgnment.AddRange(assignments);
            }
            return allAssgnment;
        }
        public async Task PublishResultAssignment(AssignmentPublishDto assignmentPublishDto)
        {
            if (assignmentPublishDto == null)
                throw new NullReferenceException("Please supply arguments");
            if (assignmentPublishDto?.AssignmentId == 0)
                throw new NullReferenceException("Please supply arguments");
            var assignment = await _context.ASSIGNMENT.Where(f => f.Id == assignmentPublishDto.AssignmentId && !f.IsDelete).FirstOrDefaultAsync();
            if (assignment == null)
                throw new NullReferenceException("Assignment not found");
            assignment.PublishResult = assignmentPublishDto.Publish;
            _context.Update(assignment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAssignment(DeleteRecordDto deleteRecordDto)
        {
            if (deleteRecordDto == null)
                throw new NullReferenceException("Please supply arguments");
            if (deleteRecordDto?.RecordId == 0)
                throw new NullReferenceException("Please supply arguments");
            var assignment = await _context.ASSIGNMENT.Where(f => f.Id == deleteRecordDto.RecordId).FirstOrDefaultAsync();
            if (assignment == null)
                throw new NullReferenceException("Assignment not found");
            assignment.IsDelete = true;
            _context.Update(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AssignmentCountBy(long UserId)
        {
            if (UserId == 0)
                throw new NullReferenceException("No User Id");
            return await _context.ASSIGNMENT
                 .Include(f => f.CourseAllocation)
                 .ThenInclude(c => c.Course)
                 .Include(f => f.CourseAllocation)
                 .ThenInclude(f => f.Instructor)
                 .ThenInclude(f => f.Person)
                 .Where(f => f.CourseAllocation.InstructorId == UserId && f.CourseAllocation.SessionSemester.Active && !f.IsDelete).CountAsync();

        }
        public async Task<StudentPersonDetailCountDto> StudentPersonStats(long PersonId)
        {
            if (PersonId == 0)
                throw new NullReferenceException("No User Id");
            StudentPersonDetailCountDto detailCountDto = new StudentPersonDetailCountDto();
            AssignmentSubmission assignmentSubmission = new AssignmentSubmission();
            Assignment assignment = new Assignment();
            Quiz Quiz = new Quiz();
            QuizSubmission QuizSubmission = new QuizSubmission();
            long _assignmentAttempted = 0;
            long _assignmentAvailable = 0;
            long _QuizAttempted = 0;
            long _QuizAvailable = 0;
            var studentPerson = await _context.STUDENT_PERSON.Where(p => p.PersonId == PersonId).FirstOrDefaultAsync();
            var _activeSessionSemester = await GetActiveSessionSemester();
            var courseRegistrationCount = await _context.COURSE_REGISTRATION.Where(x => x.StudentPersonId == studentPerson.Id && x.SessionSemester.Active).ToListAsync();
            if(courseRegistrationCount.Count > 0)
            {
                //Assignments
                foreach(var item in courseRegistrationCount)
                {
                    assignment = await _context.ASSIGNMENT.Where(x => x.CourseAllocationId == item.CourseAllocationId && x.CourseAllocation.SessionSemesterId == _activeSessionSemester.Id).FirstOrDefaultAsync();
                    if(assignment != null)
                    {
                        _assignmentAvailable += 1;
                    }
                    assignmentSubmission = await _context.ASSIGNMENT_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id).FirstOrDefaultAsync();
                    if(assignmentSubmission != null)
                    {
                        _assignmentAttempted += 1;
                    }
                }

                //Quiz
                foreach (var item in courseRegistrationCount)
                {
                    Quiz = await _context.QUIZ.Where(x => x.CourseAllocationId == item.CourseAllocationId && x.CourseAllocation.SessionSemesterId == _activeSessionSemester.Id).FirstOrDefaultAsync();
                    if (Quiz != null)
                    {
                        _QuizAvailable += 1;
                    }
                    QuizSubmission = await _context.QUIZ_SUBMISSION.Where(x => x.CourseRegistrationId == item.Id).FirstOrDefaultAsync();
                    if (QuizSubmission != null)
                    {
                        _QuizAttempted += 1;
                    }
                }
            }
            detailCountDto.CoursesRegistered = courseRegistrationCount.Count;
            detailCountDto.TotalAssignmentsAvailable = _assignmentAvailable;
            detailCountDto.AssignmentsAttempted = _assignmentAttempted;
            return detailCountDto;
        }

        public async Task<AssignmentDto> EditAssignment(UpdateAssignmentDto updateAssignmentDto)
        {
            if (updateAssignmentDto == null)
                return null;
            if (updateAssignmentDto.AssignmentId == 0)
                throw new NullReferenceException("No Assignment Id");
            var assignment = await _context.ASSIGNMENT.Where(f => f.Id == updateAssignmentDto.AssignmentId && !f.IsDelete).FirstOrDefaultAsync();
            if (assignment == null)
                throw new NullReferenceException("Assignment does not exist");
            assignment.MaxScore = updateAssignmentDto.MaxScore;
            assignment.SetDate = updateAssignmentDto.SetDate;
            assignment.DueDate = updateAssignmentDto.DueDate;
            assignment.AssignmentInstruction = updateAssignmentDto.AssignmentInstruction;
            assignment.AssignmentInText = updateAssignmentDto.AssignmentInText;
            assignment.AssignmentName = updateAssignmentDto.AssignmentName;
            _context.Update(assignment);
            var isSuccessfull = await _context.SaveChangesAsync();
            if (isSuccessfull > 0)
            {
                return await GetAssignmentByAssignmentId(assignment.Id);
            }
            return null;
        }
        public async Task<AssignmentSubmissionDto> GetAssignmentSubmissionById(long AssignmentSubmissionId)
        {
            if (AssignmentSubmissionId == 0)
                throw new NullReferenceException("No Assignment Submission Id");
             //var isPublished = await _context
           return await _context.ASSIGNMENT_SUBMISSION
                .Include(f => f.Assignment)
                .ThenInclude(f => f.CourseAllocation.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Include(f => f.CourseRegistration)
                .ThenInclude(f => f.StudentPerson)
                .ThenInclude(f => f.Person)
                .Where(f => f.Id == AssignmentSubmissionId)
                .Select(f => new AssignmentSubmissionDto
                {
                    Active = f.Active,
                    DateSubmitted = f.DateSubmitted,
                    AssignmentInTextSubmission = f.AssignmentInTextSubmission,
                    AssignmentSubmissionId = f.Id,
                    AssignmentSubmissionUploadLink = f.AssignmentSubmissionUploadLink != null ? baseUrl + f.AssignmentSubmissionUploadLink : null,
                    InstructorRemark = f.InstructorRemark,
                    Score = f.Score,
                    StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                    MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                    AssignmentSubmissionHostedLink = f.AssignmentSubmissionHostedLink,
                    IsPublished = f.Assignment.PublishResult,
                    AssignmentId = f.Assignment.Id,
                    MaxCharacters = f.Assignment.MaxCharacters
                    

                })
                .FirstOrDefaultAsync();
        }
        public async Task<AssignmentDto> GetAssignmentByAssignmentId(long AssignmentId)
        {
            if (AssignmentId == 0)
                throw new NullReferenceException("No Assignment Id");
            return await _context.ASSIGNMENT.Where(f => f.Id == AssignmentId)
                .Include(f => f.CourseAllocation.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new AssignmentDto
                {
                    AssignmentId = f.Id,
                    Active = f.Active,
                    AssignmentName = f.AssignmentName,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    DueDate = f.DueDate,
                    InstructorName = (f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername),
                    IsPublished = f.PublishResult,
                    MaxScore = f.MaxScore,
                    AssignmentInstruction = f.AssignmentInstruction,
                    AssignmentInText = f.AssignmentInText,
                    AssignmentUploadLink = f.AssignmentUploadLink != null ? baseUrl + f.AssignmentUploadLink :null,
                    IsDeleted = f.IsDelete,
                    AssignmentVideoLink = f.AssignmentVideoLink,
                    SetDate = f.SetDate,
                    MaxCharacters = f.MaxCharacters
                    

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
        public async Task<IEnumerable<AssignmentSubmissionDto>> GetAllAssignmentSubmissionByAssignemntId(long AssignmentId)
        {
            if (AssignmentId == 0)
                throw new NullReferenceException("No Assignment Id");
            //var isPublished = await _context
            return await _context.ASSIGNMENT_SUBMISSION
                 .Include(f => f.Assignment)
                 .ThenInclude(f => f.CourseAllocation.Course)
                 .ThenInclude(f => f.User)
                 .ThenInclude(f => f.Person)
                 .Include(f => f.CourseRegistration)
                 .ThenInclude(f => f.StudentPerson)
                 .ThenInclude(f => f.Person)
                 .Where(f => f.AssignmentId == AssignmentId)
                 .Select(f => new AssignmentSubmissionDto
                 {
                     Active = f.Active,
                     DateSubmitted = f.DateSubmitted,
                     AssignmentInTextSubmission = f.AssignmentInTextSubmission,
                     AssignmentSubmissionId = f.Id,
                     AssignmentSubmissionUploadLink = f.AssignmentSubmissionUploadLink != null ? baseUrl + f.AssignmentSubmissionUploadLink : null,
                     InstructorRemark = f.InstructorRemark,
                     Score = f.Score,
                     StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                     MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                     AssignmentSubmissionHostedLink = f.AssignmentSubmissionHostedLink,
                     IsPublished = f.Assignment.PublishResult,
                     AssignmentId = f.Assignment.Id,
                     LateSubmission = f.IsLateSubmission
                 })
                 .ToListAsync();
        }

        public async Task<AssignmentSubmissionDto> GetAssignmentSubmissionBy(long AssignmentId, long StudentUserId)
        {
            try
            {
                if (AssignmentId == 0 || StudentUserId == 0)
                    throw new NullReferenceException("No Assignment Submission Id");
                var getUser = await _context.USER.Where(i => i.Id == StudentUserId).Include(p => p.Person).FirstOrDefaultAsync();

                //var isPublished = await _context
                return await _context.ASSIGNMENT_SUBMISSION
                     .Include(f => f.Assignment)
                     .ThenInclude(f => f.CourseAllocation.Course)
                     .ThenInclude(f => f.User)
                     .ThenInclude(f => f.Person)
                     .Include(f => f.CourseRegistration)
                     .ThenInclude(f => f.StudentPerson)
                     .ThenInclude(f => f.Person)
                     .Where(f => f.AssignmentId == AssignmentId && f.CourseRegistration.StudentPerson.Person.Id == getUser.Person.Id)
                     .Select(f => new AssignmentSubmissionDto
                     {
                         Active = f.Active,
                         DateSubmitted = f.DateSubmitted,
                         AssignmentInTextSubmission = f.AssignmentInTextSubmission,
                         AssignmentSubmissionId = f.Id,
                         AssignmentSubmissionUploadLink = f.AssignmentSubmissionUploadLink != null ? baseUrl + f.AssignmentSubmissionUploadLink : null,
                         InstructorRemark = f.InstructorRemark,
                         Score = f.Score,
                         StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                         MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                         AssignmentSubmissionHostedLink = f.AssignmentSubmissionHostedLink,
                         IsPublished = f.Assignment.PublishResult,
                         AssignmentId = f.Assignment.Id,
                         MaxCharacters = f.Assignment.MaxCharacters
                     })
                     .FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public async Task<ResponseModel> ExtendAssignmentDueDate(AssignmentDueDateDto dto)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var assignment = await _context.ASSIGNMENT.Where(x => x.Id == dto.AssignmentId).FirstOrDefaultAsync();
                if(assignment != null)
                {
                    assignment.DueDate = dto.DueDate;
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();                    
                    return response;
                }
                return null;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                return response;
            }
        }       
    }
}
