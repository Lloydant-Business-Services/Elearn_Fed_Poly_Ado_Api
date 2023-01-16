using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService : IRepository<User>
    {

        Task<UserDto> AuthenticateUser(UserDto dto, string injectkey);
        //Task<long> PostUser(AddUserDto userDto);
        Task<bool> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<GetUserProfileDto> GetUserProfile(long userId);
        Task<ResponseModel> ProfileUpdate(UpdateUserProfileDto dto);
        Task<bool> AscertainMultiRole(long userId, long sessionSemesterId);
        Task<int> ResetPassword(string Username);
        Task<bool> ValidateOTP(string email, string otp);
        Task<bool> UpdatePasswordAfterReset(ChangePasswordDto changePasswordDto);
        Task<bool> CreateNotificationTracker(NotificationTracker model);
        Task<IEnumerable<GetNotificationTrackerDto>> GetNotificationTrackersByUserId(long userId);
        Task<bool> ToggleMailRead(long notificationTrackerId);
        Task<ResponseModel> DelegateTaskToSubAdmin(long userId, TaskDelegations task);
        Task<IEnumerable<SubAdminDelegationDto>> GetAllSubAdmins();
        Task<bool> RevokeAdminDelegation(long delegationId);
        //Task<IEnumerable<GetInstitutionUsersDto>> GetAllStudents();
    }
}
