using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IReportingSevice
    {
        Task<IEnumerable<GetInstructorDto>> GetInstructors(long departmentId, long sessionSemesterId);
        Task<IEnumerable<StudentReportListDto>> GetStudentsBy(long DepartmentId, long SessionSemesterId);
        Task<IEnumerable<AssignmentCumulativeDto>> AssignmentCumulativeScore(long courseId, long sessionSemesterId, long departmentId);
        Task<IEnumerable<ComprehensiveAssignmentCumulativeDto>> ComprehensiveAssignmentReport(long courseId, long sessionSemesterId, long departmentId);
        Task<StudentCumulativeAssignmentModel> CumulativeComprehensiveAssignmentReportByStudent(long personId, long sessionSemesterId);
        Task<IEnumerable<ComprehensiveAssignmentCumulativeDto>> ComprehensiveQuizReport(long courseId, long sessionSemesterId, long departmentId);
    }
}
