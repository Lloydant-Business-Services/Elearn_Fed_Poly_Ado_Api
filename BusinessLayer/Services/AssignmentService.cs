using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AssignmentService : ICourseAssignment
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string baseUrl;
        private readonly IFileUpload _fileUpload;


        public AssignmentService(IConfiguration configuration, ELearnContext context, IFileUpload fileUpload)
        {
            _context = context;
            _fileUpload = fileUpload;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");
        }

        public async Task<AssignmentDto> AddAssignment(AddAssignmentDto addAssignmentDto, string filePath, string directory)
        {
            if (addAssignmentDto == null)
                return null;
            if (addAssignmentDto.CourseAllocationId == 0)
                throw new NullReferenceException("No Course Id");
            var course = await _context.COURSE_ALLOCATION.Where(f => f.Id == addAssignmentDto.CourseAllocationId && !f.Active).FirstOrDefaultAsync();
            if (course == null)
                throw new NullReferenceException("Course not allocated");
            var saveAssignmentLink = string.Empty;
            if (addAssignmentDto.AssignmentUpload != null)
            {

                addAssignmentDto.Name = addAssignmentDto.Name.Replace(" ", "");
                string fileNamePrefix = course.Course.CourseCode + "_" + addAssignmentDto.Name + "_" + DateTime.Now.Millisecond;
                saveAssignmentLink = await _fileUpload.GetNoteUploadLink(addAssignmentDto.AssignmentUpload, filePath, directory, fileNamePrefix);
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
                PublishResult = false
            };
            _context.Add(assignment);
            var isSuccessfull = await _context.SaveChangesAsync();
            if (isSuccessfull > 0)
            {
                return await GetAssignmentByAssignmentId(assignment.Id);
            }
            return null;

        }

        public async Task<AssignmentSubmissionDto> AddAssignmentSubmission(StudentAssignmentSubmissionDto studentAssignmentSubmissionDto, string filePath, string directory)
        {
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
            var saveAssignmentLink = string.Empty;
            if (studentAssignmentSubmissionDto.AssignmentUpload != null)
            {
                assignment.CourseAllocation.Course.CourseCode = assignment.CourseAllocation.Course.CourseCode.Replace(" ", "");
                string fileNamePrefix = user.Person.Surname + "_" + user.Person.Firstname + "_" + assignment.CourseAllocation.Course.CourseCode + "_" + DateTime.Now.Millisecond;
                saveAssignmentLink = await _fileUpload.GetNoteUploadLink(studentAssignmentSubmissionDto.AssignmentUpload, filePath, directory, fileNamePrefix);
            }
            var courseRegistration = await _context.COURSE_REGISTRATION.Where(f => f.CourseAllocation.CourseId == assignment.CourseAllocation.CourseId && f.StudentPerson.PersonId == user.PersonId).FirstOrDefaultAsync();
            if (courseRegistration == null)
                throw new NullReferenceException("Student did not register this course");
            AssignmentSubmission assignmentSubmission = new AssignmentSubmission()
            {
                Active = true,
                Assignment = assignment,
                DateSubmitted = DateTime.Now,
                AssignmentInTextSubmission = studentAssignmentSubmissionDto.AssignmentInText,
                AssignmentSubmissionHostedLink = studentAssignmentSubmissionDto.AssignmentHostedLink,
                AssignmentSubmissionUploadLink = saveAssignmentLink,
                CourseRegistration = courseRegistration,
            };
            _context.Add(assignmentSubmission);
            var isSuccessful = await _context.SaveChangesAsync();
            if (isSuccessful > 0)
            {
                return await GetAssignmentSubmissionById(assignmentSubmission.Id);
            }
            return null;
        }

        public async Task<AssignmentSubmissionDto> GradeAssignment(GradeAssignmentDto gradeAssignmentDto)
        {
            if (gradeAssignmentDto == null)
                throw new NullReferenceException("No Content");
            if (gradeAssignmentDto?.AssignmentSubmissionId == 0)
                throw new NullReferenceException("No Assignment Submission Id");
            var assignmentSubmission = await _context.ASSIGNMENT_SUBMISSION.Where(f => f.Id == gradeAssignmentDto.AssignmentSubmissionId).FirstOrDefaultAsync();
            if (assignmentSubmission == null)
                throw new NullReferenceException("No Assignment Submission Record");
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
                    MaxScore = f.MaxScore
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
                    MaxScore = f.MaxScore
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
                .Where(f => f.Active && f.StudentPerson.PersonId == user.PersonId && f.CourseAllocationId == institutionActiveSessionSemester.Id).ToListAsync();
            if (institutionActiveSessionSemester == null)
                return null;
            //get all the assignment for the student based on the registered course
            foreach (var item in courseRegistration)
            {
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
                   MaxScore = f.MaxScore
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
                    AssignmentSubmissionUploadLink = baseUrl + f.AssignmentSubmissionUploadLink,
                    InstructorRemark = f.InstructorRemark,
                    Score = f.Score,
                    StudentName = (f.CourseRegistration.StudentPerson.Person.Surname + " " + f.CourseRegistration.StudentPerson.Person.Firstname + " " + f.CourseRegistration.StudentPerson.Person.Othername),
                    MatricNumber = f.CourseRegistration.StudentPerson.MatricNo,
                    AssignmentSubmissionHostedLink = f.AssignmentSubmissionHostedLink

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
                    AssignmentUploadLink = baseUrl + f.AssignmentUploadLink,
                    IsDeleted = f.IsDelete,
                    AssignmentVideoLink = f.AssignmentVideoLink,
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

    }
}
