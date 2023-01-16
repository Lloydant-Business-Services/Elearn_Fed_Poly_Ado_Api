using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
   public interface ICourseMaterialService
    {
        Task<long> CreateCourseTopic(AddCourseTopicDto addCourseTopicDto);
        Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicBy(long CourseAllocationId);
        Task<long> AddCourseContent(AddCourseContentDto addCourseContentDto, string filePath, string directory);
        Task<IEnumerable<GetCourseContentDto>> GetContentBy(long TopicId);
        Task<bool> DeleteCourseContent(long courseContentId);
        Task<bool> DeleteCourseTopic(long TopicId);
        Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId);
        Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByInstructorId(long InstructorId);
       Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByCourseId(long CourseId);
       Task<IEnumerable<GetCourseContentDto>> GetCourseMaterialByDepartmentId(long DepartmentId);
       Task<bool> EditCourseTopic(long TopicId, AddCourseTopicDto dto);
        Task<IEnumerable<GetCourseContentDto>> GetAllContentUserId(long userId);
        Task<IEnumerable<GetCourseTopicDto>> GetCourseTopicByInstructor(long userId);
        Task<bool> ImportCourseMaterialByTopic(long contentId, long newTopicId);
    }
}
