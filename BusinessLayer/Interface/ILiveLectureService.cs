using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ILiveLectureService
    {
        Task<int> OAuthRedirect(string code);
        Task<int> GetUserDetails(string accessToken);
        Task<int> CreateMeeting(MeetingDto meetingDto);
        Task<IEnumerable<MeetingDto>> GetMeetingByCourseId(long courseId);
        Task<IEnumerable<MeetingDto>> GetLiveLecturesByInstructor(long InstructorId);
        Task<string> ZoomSignIn();
       Task<IEnumerable<MeetingDto>> GetAllLiveLectures();
       Task<int> DeleteLiveLecture(long LiveLectureId);
       Task<int> ToggleClassOnlineStatus(long liveLectureId, bool status);
       Task<IEnumerable<MeetingDto>> GetActiveLiveLectures();
       Task<IEnumerable<MeetingDto>> GetActiveLiveLecturesByDepartment(long departmentId);
    }
}
