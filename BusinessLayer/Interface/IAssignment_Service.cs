using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAssignment_Service
    {
        Task<AssignmentDto> AddAssignment(AddAssignmentDto addAssignmentDto, string filePath, string directory);
        Task<AssignmentDto> GetAssignmentByAssignmentId(long AssignmentId);
        Task<ResponseModel> AddAssignmentSubmission(StudentAssignmentSubmissionDto studentAssignmentSubmissionDto, string filePath, string directory);
        Task<AssignmentSubmissionDto> GradeAssignment(GradeAssignmentDto gradeAssignmentDto);
        Task<IEnumerable<AssignmentListDto>> ListAssignmentByCourseId(long courseId);
        Task<IEnumerable<AssignmentListDto>> ListAssignmentByInstructorUserId(long userId);
        Task<IEnumerable<AssignmentListDto>> ListAssignmentByStudentId(long StudentUserId);
        Task PublishResultAssignment(AssignmentPublishDto assignmentPublishDto);
        Task DeleteAssignment(DeleteRecordDto deleteRecordDto);
        Task<int> AssignmentCountBy(long UserId);
        Task<AssignmentDto> EditAssignment(UpdateAssignmentDto updateAssignmentDto);
        Task<AssignmentSubmissionDto> GetAssignmentSubmissionById(long AssignmentSubmissionId);
        Task<StudentPersonDetailCountDto> StudentPersonStats(long PersonId);
        Task<IEnumerable<AssignmentSubmissionDto>> GetAllAssignmentSubmissionByAssignemntId(long AssignmentId);
        Task<AssignmentSubmissionDto> GetAssignmentSubmissionBy(long AssignmentId, long StudentUserId);
       Task<ResponseModel> ExtendAssignmentDueDate(AssignmentDueDateDto dto);
    }
}
