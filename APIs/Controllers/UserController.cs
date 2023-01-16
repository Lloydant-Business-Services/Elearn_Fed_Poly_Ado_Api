using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APIs.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");
        }

        [HttpPost("Authenticate")]
        public async Task<UserDto> AuthenticateUser(UserDto dto) => await _userService.AuthenticateUser(dto, key);
        //[HttpPost("AddUser")]      
        //public async Task<long> PostUser(AddUserDto userDto) => await _userService.PostUser(userDto);

        [HttpPost("ChangePassword")]
        public async Task<bool> ChangeUserPassword(ChangePasswordDto changePasswordDto) => await _userService.ChangePassword(changePasswordDto);

        [HttpGet("UserProfile")]
        public async Task<GetUserProfileDto> GetUserProfile(long userId) => await _userService.GetUserProfile(userId);
        [HttpPost("[action]")]
        public async Task<ResponseModel> ProfileUpdate(UpdateUserProfileDto dto) => await _userService.ProfileUpdate(dto);
        [HttpGet("[action]")]
        public async Task<bool> AscertainMultiRole(long userId, long sessionSemesterId) => await _userService.AscertainMultiRole(userId, sessionSemesterId);
        [HttpPost("[action]")]
        public async Task<int> ResetPassword(string Username) => await _userService.ResetPassword(Username);
        [HttpPost("[action]")]
        public async Task<bool> ValidateOTP(string email, string otp) => await _userService.ValidateOTP(email, otp);
        [HttpPost("[action]")]
        public async Task<bool> UpdatePasswordAfterReset(ChangePasswordDto changePasswordDto) => await _userService.UpdatePasswordAfterReset(changePasswordDto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetNotificationTrackerDto>> GetNotificationTrackersByUserId(long userId) => await _userService.GetNotificationTrackersByUserId(userId);
        [HttpPost("[action]")]
        public async Task<bool> ToggleMailRead(long notificationTrackerId) => await _userService.ToggleMailRead(notificationTrackerId);

        [HttpPost("[action]")]
        public async Task<ResponseModel> DelegateTaskToSubAdmin(long userId, TaskDelegations task) => await _userService.DelegateTaskToSubAdmin(userId, task);

        [HttpGet("[action]")]
        public async Task<IEnumerable<SubAdminDelegationDto>> GetAllSubAdmins() => await _userService.GetAllSubAdmins();

        [HttpPost("[action]")]
        public async Task<bool> RevokeAdminDelegation(long delegationId) => await _userService.RevokeAdminDelegation(delegationId);

    }
}
