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
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string baseUrl;

        public CourseMaterialService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }


        public async Task<long> CreateCourseTopic(AddCourseTopicDto addCourseTopicDto)
        {
            if (addCourseTopicDto?.CourseAllocationId > 0 && addCourseTopicDto?.TopicName != null && addCourseTopicDto?.StartDate != null)
            {
                var course = await _context.COURSE_ALLOCATION.Where(f => f.Id == addCourseTopicDto.CourseAllocationId && f.Active).FirstOrDefaultAsync();
                if (course == null)
                    throw new NullReferenceException("Selected Course does not exist");
                CourseTopic courseTopic = new CourseTopic()
                {
                    Description = addCourseTopicDto.TopicDescription,
                    StartDate = addCourseTopicDto.StartDate,
                    EndDate = addCourseTopicDto.EndDate,
                    SetDate = DateTime.UtcNow,
                    CourseAllocationId = course.Id,
                    Active = true,
                    IsArchieved = false,
                    Topic = addCourseTopicDto.TopicName,

                };
                _context.Add(courseTopic);
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                {
                    return StatusCodes.Status200OK;
                }
            }
            else
            {
                throw new NullReferenceException("Please, provide all the required fields");
            }
            return 0;
        }

        public async Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicBy(long CourseAllocationId)
        {
            if (CourseAllocationId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseTopics = await _context.COURSE_TOPIC.Where(f => f.CourseAllocationId == CourseAllocationId && !f.IsArchieved)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Select(f => new GetCourseTopicDto
                {
                    //CourseAccessCode = f.CourseAllocation.CourseAccessCode,
                    SetDate = f.SetDate,
                    StartDate = f.StartDate,
                    TopicDescription = f.Description,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseId = f.CourseAllocation.Course.Id,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    TopicId = f.Id,
                    TopicName = f.Topic,
                    InstructorEmail = f.CourseAllocation.Instructor.Username,
                    InstructorName = f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername
                })
                .ToListAsync();
            return courseTopics;
        }

        public async Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicByInstructor(long userId)
        {
            if (userId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseTopics = await _context.COURSE_TOPIC.Where(f => f.CourseAllocation.InstructorId == userId && f.CourseAllocation.SessionSemester.Active && !f.IsArchieved)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.Course)
                .ThenInclude(f => f.User)
                .ThenInclude(f => f.Person)
                .Include(f => f.CourseAllocation)
                .ThenInclude(c => c.SessionSemester)
                .Select(f => new GetCourseTopicDto
                {
                    //CourseAccessCode = f.CourseAllocation.CourseAccessCode,
                    SetDate = f.SetDate,
                    StartDate = f.StartDate,
                    TopicDescription = f.Description,
                    CourseCode = f.CourseAllocation.Course.CourseCode,
                    CourseId = f.CourseAllocation.Course.Id,
                    CourseTitle = f.CourseAllocation.Course.CourseTitle,
                    TopicId = f.Id,
                    TopicName = f.Topic,
                    InstructorEmail = f.CourseAllocation.Instructor.Username,
                    InstructorName = f.CourseAllocation.Instructor.Person.Surname + " " + f.CourseAllocation.Instructor.Person.Firstname + " " + f.CourseAllocation.Instructor.Person.Othername
                })
                .ToListAsync();
            return courseTopics;
        }

        public async Task<bool> ImportCourseMaterialByTopic(long contentId, long newTopicId)
        {
            var getContent = await _context.COURSE_CONTENT.Where(x => x.Id == contentId).FirstOrDefaultAsync();
            if(getContent != null)
            {
                CourseContent courseContent = new CourseContent()
                {
                    CourseTopicId = newTopicId,
                    Material = getContent.Material,
                    ContentTitle = getContent.ContentTitle,
                    Active = true,  
                };
                _context.Add(courseContent);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<long> AddCourseContent(AddCourseContentDto addCourseContentDto, string filePath, string directory)
        {
            if (addCourseContentDto?.CourseTopicId > 0)
            {
                var courseTopic = await _context.COURSE_TOPIC
                    .Include(f => f.CourseAllocation)
                    .ThenInclude(c => c.Course)
                    .Where(f => f.Id == addCourseContentDto.CourseTopicId).FirstOrDefaultAsync();
                if (courseTopic == null)
                    throw new NullReferenceException("Course Topic does not exist");
                var saveNoteLink = string.Empty;
                if (addCourseContentDto.Note != null)
                {
                    string fileNamePrefix = courseTopic.Topic + "_" + courseTopic.CourseAllocationId + "_" + DateTime.Now.Millisecond;
                    saveNoteLink = await GetNoteUploadLink(addCourseContentDto.Note, filePath, directory, fileNamePrefix);
                }

                CourseContent courseContent = new CourseContent()
                {
                    Active = true,
                    CourseTopic = courseTopic,
                    Link = addCourseContentDto.VideoLink,
                    SetDate = DateTime.UtcNow,
                    LiveStream = addCourseContentDto.StreamLink,
                    Material = saveNoteLink,
                    ContentTitle = addCourseContentDto.ContentTitle
                };
                _context.Add(courseContent);
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                    return StatusCodes.Status200OK;
            }
            else
            {
                throw new NullReferenceException("Please, select topic to continue");
            }
            return 0;
        }

        public async Task<IEnumerable<GetCourseContentDto>> GetContentBy(long TopicId)
        {
            if (TopicId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseContentDtos = await _context.COURSE_CONTENT
                .Include(f => f.CourseTopic)
                .Where(f => f.CourseTopicId == TopicId && !f.IsArchieved)
                .Select(f => new GetCourseContentDto
                {
                    LiveStreamLink = f.LiveStream,
                    NoteLink = !string.IsNullOrWhiteSpace(f.Material) ? baseUrl + f.Material : null,
                    TopicDescription = f.CourseTopic.Description,
                    StartTime = f.CourseTopic.StartDate,
                    TopicName = f.CourseTopic.Topic,
                    VideoLink = f.Link,
                    ContentTitle = f.ContentTitle,
                    
                })
                .ToListAsync();
            return courseContentDtos;
        }

        public async Task<IEnumerable<GetCourseContentDto>> GetAllContentUserId(long userId)
        {
            if (userId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseContentDtos = await _context.COURSE_CONTENT
                .Include(f => f.CourseTopic)
                .ThenInclude(x => x.CourseAllocation)
                .ThenInclude(x => x.SessionSemester)
                .Where(f => f.CourseTopic.CourseAllocation.InstructorId == userId && !f.CourseTopic.CourseAllocation.SessionSemester.Active)
                .Select(f => new GetCourseContentDto
                {
                    LiveStreamLink = f.LiveStream,
                    NoteLink = !string.IsNullOrWhiteSpace(f.Material) ? baseUrl + f.Material : null,
                    TopicDescription = f.CourseTopic.Description,
                    StartTime = f.CourseTopic.StartDate,
                    TopicName = f.CourseTopic.Topic,
                    VideoLink = f.Link,
                    ContentTitle = f.ContentTitle,

                })
                .ToListAsync();
            return courseContentDtos;
        }

        public async Task<bool> DeleteCourseContent(long courseContentId)
        {
            if (courseContentId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseContent = await _context.COURSE_CONTENT.Where(f => f.Id == courseContentId).FirstOrDefaultAsync();
            if (courseContent?.Id > 0)
            {
                courseContent.IsArchieved = true;
                _context.Update(courseContent);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteCourseTopic(long TopicId)
        {
            if (TopicId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseTopic = await _context.COURSE_TOPIC.Where(f => f.Id == TopicId).FirstOrDefaultAsync();
            if (courseTopic?.Id > 0)
            {
                var courseContent = await _context.COURSE_CONTENT.Where(f => f.CourseTopicId == TopicId).ToListAsync();
                if(courseContent != null && courseContent.Count > 0)
                {
                    foreach(var item in courseContent)
                    {
                        item.IsArchieved = true;
                        _context.Update(item);
                    }
                }
                courseTopic.IsArchieved = true;
                _context.Update(courseTopic);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EditCourseTopic(long TopicId, AddCourseTopicDto dto)
        {
            if (TopicId == 0)
                throw new NullReferenceException("Please provide query parameter");
            var courseTopic = await _context.COURSE_TOPIC.Where(f => f.Id == TopicId).FirstOrDefaultAsync();
            if (courseTopic?.Id > 0)
            {
                courseTopic.Topic = dto.TopicName;
                courseTopic.Description = dto.TopicDescription;
                courseTopic.StartDate = dto.StartDate;
                courseTopic.EndDate = dto.EndDate;

                _context.Update(courseTopic);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<string> GetNoteUploadLink(IFormFile file, string filePath, string directory, string givenFileName)
        {

            var noteUrl = string.Empty;

            var validFileSize = 10000000;//10mb
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
                        throw new BadImageFormatException("Invalid file type...Accepted formats are jpg, jpeg, and png");
                    }
                }
            }
            return noteUrl;
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

        public async Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByInstructorId(long InstructorId)
        {
            var courseContentDtos = await _context.COURSE_CONTENT
                .Include(f => f.CourseTopic)
                .ThenInclude(c => c.CourseAllocation)
                .ThenInclude(c => c.SessionSemester)
                .Where(f => f.CourseTopic.CourseAllocation.InstructorId == InstructorId && !f.CourseTopic.CourseAllocation.SessionSemester.Active && !f.IsArchieved)
                .Select(f => new GetCourseContentDto
                {
                    LiveStreamLink = f.LiveStream,
                    NoteLink = !string.IsNullOrWhiteSpace(f.Material) ? baseUrl + f.Material : null,
                    TopicDescription = f.CourseTopic.Description,
                    StartTime = f.CourseTopic.StartDate,
                    TopicName = f.CourseTopic.Topic,
                    VideoLink = f.Link,
                    ContentTitle = f.ContentTitle,
                    TopicId = f.CourseTopic.Id,
                    ContentId = f.Id

                })
                .ToListAsync();
            return courseContentDtos;
        }

        public async Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByCourseId(long CourseId)
        {
            var courseContentDtos = await _context.COURSE_CONTENT
                .Include(f => f.CourseTopic)
                .ThenInclude(c => c.CourseAllocation)
                .Where(f => f.CourseTopic.CourseAllocation.CourseId == CourseId && f.CourseTopic.IsArchieved == false && !f.IsArchieved)
                .Select(f => new GetCourseContentDto
                {
                    LiveStreamLink = f.LiveStream,
                    NoteLink = !string.IsNullOrWhiteSpace(f.Material) ? baseUrl + f.Material : null,
                    TopicDescription = f.CourseTopic.Description,
                    StartTime = f.CourseTopic.StartDate,
                    TopicName = f.CourseTopic.Topic,
                    VideoLink = f.Link,
                    ContentTitle = f.ContentTitle,
                    TopicId = f.CourseTopic.Id,
                    ContentId = f.Id

                })
                .ToListAsync();
            return courseContentDtos;
        }
        public async Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByDepartmentId(long DepartmentId)
        {
            List<GetCourseContentDto> aggList = new List<GetCourseContentDto>();
            var instructorDept = await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == DepartmentId).ToListAsync();

            if (instructorDept.Count > 0)
            {
                foreach (var item in instructorDept)
                {
                    var courseContentDtos = await _context.COURSE_CONTENT
                        .Include(f => f.CourseTopic)
                        .ThenInclude(c => c.CourseAllocation)
                        .Where(f => f.CourseTopic.CourseAllocationId == item.CourseAllocationId && f.CourseTopic.IsArchieved == false && !f.IsArchieved)
                        .Select(f => new GetCourseContentDto
                        {
                            LiveStreamLink = f.LiveStream,
                            NoteLink = !string.IsNullOrWhiteSpace(f.Material) ? baseUrl + f.Material : null,
                            TopicDescription = f.CourseTopic.Description,
                            StartTime = f.CourseTopic.StartDate,
                            TopicName = f.CourseTopic.Topic,
                            VideoLink = f.Link,
                            ContentTitle = f.ContentTitle,
                            TopicId = f.CourseTopic.Id,
                            ContentId = f.Id

                        })
                        .ToListAsync();
                    aggList.AddRange(courseContentDtos);
                }
                return aggList;
            }
            return null;
        }

    }
}
