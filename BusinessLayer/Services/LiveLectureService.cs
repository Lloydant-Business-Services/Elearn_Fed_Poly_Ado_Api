using BusinessLayer.Interface;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Enums;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class LiveLectureService : ILiveLectureService
    {
        private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        
        private readonly string baseUrl;
        ZoomParam zoomParam = new ZoomParam();
        string startupPath = System.IO.Directory.GetCurrentDirectory();
        string tokenFilePath = Environment.CurrentDirectory + "//OauthToken.json";
        string userTokenFilePath = Environment.CurrentDirectory + "//UserDetails.json";
        string meetingRsponsePath = Environment.CurrentDirectory + "//MeetingResponsePath.json";

        public LiveLectureService(ELearnContext context, IConfiguration configuration, IEmailService emailService, IUserService userService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _userService = userService;
            baseUrl = _configuration.GetValue<string>("Url:root");
        }
        private string AuthorizationHeader
        {
            get
            {

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(zoomParam.ClientId + ":" + zoomParam.ClientSecret);
                var encodedString = Convert.ToBase64String(plainTextBytes);
                return $"{encodedString}";
                //return $"Basic {encodedString}";
            }
        }
        public async Task<string> ZoomSignIn()
        {
            ZoomParam zoomParam = new ZoomParam();
            var auth_url = zoomParam.AuthorizationUrl;
            return auth_url;
        }
        public async Task<int> OAuthRedirect(string code)
        {
            //var sta = startupPath + "//Data//DTO//OauthToken.json";
            RestClient restClient = new RestClient();
            var request = new RestRequest();
            var acc_Token_url = $"https://zoom.us/oauth/token?grant_type=authorization_code&code={code}&redirect_uri=http://blurapp.pixufy.com/login";
            //var acc_Token_url = $"https://zoom.us/oauth/token?grant_type=authorization_code&code={code}&redirect_uri=https://applications.federalpolyilaro.edu.ng/Security/Account/Login";
            restClient.BaseUrl = new Uri(acc_Token_url);
            request.AddHeader("Authorization", "Basic " + AuthorizationHeader);
            //request.AddHeader("Authorization", string.Format(AuthorizationHeader));

            var response = restClient.Post(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(tokenFilePath, response.Content);
                var token = JObject.Parse(response.Content);
                var acc = GetUserDetails(token["access_token"].ToString());
                return StatusCodes.Status200OK;
            }
            return StatusCodes.Status400BadRequest;

        }


        public async Task<int> GetUserDetails(string accessToken)
        {
            RestClient restClient = new RestClient();
            var request = new RestRequest();
            restClient.BaseUrl = new Uri("https://api.zoom.us/v2/users/me");
            request.AddHeader("Authorization", string.Format("Bearer {0}", accessToken));
            var response = restClient.Get(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(userTokenFilePath, response.Content);
                var userToken = JObject.Parse(response.Content);
                //return userToken;
                return StatusCodes.Status200OK;
            }
            return StatusCodes.Status400BadRequest;

        }

        public async Task<int> CreateMeeting(MeetingDto meetingDto)
        {
            //var token = JObject.Parse(System.IO.File.ReadAllText(tokenFilePath));
            //var userDetails = JObject.Parse(System.IO.File.ReadAllText(userTokenFilePath));
            //var access_token = token["access_token"];
            //var userId = userDetails["id"];

            //var meetingModel = new JObject();
            //meetingModel["topic"] = meetingDto.Topic;
            //meetingModel["agenda"] = meetingDto.Agenda;
            //meetingModel["start_time"] = meetingDto.Date.ToString("yyyy-mm-dd") + "T" + TimeSpan.FromHours(meetingDto.Time).ToString();
            //meetingModel["duration"] = meetingDto.Duration;

            //var meeting_url = $"https://api.zoom.us/v2/users/{userId}/meetings";

            //var model = JsonConvert.SerializeObject(meetingModel);
            //RestRequest restRequest = new RestRequest();
            //restRequest.AddHeader("Content-Type", "application/json");
            //restRequest.AddHeader("Authorization", string.Format("Bearer {0}", access_token));
            //restRequest.AddParameter("application/json", model, ParameterType.RequestBody);

            //RestClient restClient = new RestClient();
            //restClient.BaseUrl = new Uri(meeting_url);
            //var response = restClient.Post(restRequest);
            //if (response.StatusCode == System.Net.HttpStatusCode.Created)
            //{
                ClassMeetings classMeetings = new ClassMeetings();
                var courseExists = await _context.COURSE_ALLOCATION.Where(c => c.Id == meetingDto.CourseAllocationId).FirstOrDefaultAsync();
                var userExists = await _context.USER.Where(c => c.Id == meetingDto.UserId).FirstOrDefaultAsync();

                if ((courseExists != null && courseExists.Id > 0) && (userExists != null && userExists.Id > 0))
                {
                    //var json_response = JObject.Parse(response.Content);
                    //var join_url = json_response["join_url"].ToString();
                    //var start_url = json_response["start_url"].ToString();
                    //var json_response = JsonConvert.SerializeObject(response.Content);

                    classMeetings.Topic = meetingDto.Topic;
                    classMeetings.Date = meetingDto.Date;
                    classMeetings.Duration = meetingDto.Duration;
                    classMeetings.Time = meetingDto.Time;
                    classMeetings.StartTime = meetingDto.StartTime;
                    classMeetings.Agenda = meetingDto.Agenda;
                    classMeetings.UserId = meetingDto.UserId;
                    classMeetings.CourseAllocationId = meetingDto.CourseAllocationId;
                    classMeetings.Start_Meeting_Url = meetingDto.Join_Meeting_Url;
                    classMeetings.Join_Meeting_Url = meetingDto.Join_Meeting_Url;
                    _context.Add(classMeetings);
                    await _context.SaveChangesAsync();
                }
            List<CourseRegistration> courseRegistration = await _context.COURSE_REGISTRATION.Where(x => x.CourseAllocationId == meetingDto.CourseAllocationId && x.SessionSemester.Active)
                .Include(x => x.StudentPerson)
                .ThenInclude(x => x.Person)
                .Include(x => x.CourseAllocation)
                .ThenInclude(x => x.Course)
                .Include(x => x.SessionSemester).ToListAsync();
            if (courseRegistration.Any())
            {
                foreach(var item in courseRegistration)
                {
                    EmailDto emailDto = new EmailDto()
                    {
                        
                        Subject = "New Live Lecture",
                        ReceiverEmail = item.StudentPerson.Person.Email,
                        NotificationCategory = EmailNotificationCategory.LiveLectureAlert,
                        ReceiverName = item.StudentPerson.Person.Firstname,
                        message =  "A live lecture for " + item.CourseAllocation.Course.CourseTitle + " " + item.CourseAllocation.Course.CourseCode + " has been scheduled for " + classMeetings.Date.ToLongDateString()
                    };
                    var sendOTPViaEmail = _emailService.EmailFormatter(emailDto);
                    NotificationTracker notificationTracker = new NotificationTracker()
                    {
                        PersonId = item.StudentPerson.Person.Id,
                        EmailNotificationCategory = EmailNotificationCategory.LiveLectureAlert,
                        NotificationDescription = emailDto.message,
                        TItle = emailDto.Subject,
                        DateAdded = DateTime.Now,
                        Active = true,
                        Person = item.StudentPerson.Person
                    };
                    await _userService.CreateNotificationTracker(notificationTracker);
                }
            }
                //System.IO.File.WriteAllText(meetingRsponsePath, response.Content);
                return StatusCodes.Status200OK;
            //}
            //else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    var isRefreshed = await RefreshToken(meetingDto);
            //    if (isRefreshed)
            //    {
            //        return StatusCodes.Status200OK;
            //    }
            //}
            return StatusCodes.Status400BadRequest;

        }


        public async Task<IEnumerable<MeetingDto>> GetMeetingByCourseId(long courseId)
        {
            MeetingDto meetingDto = new MeetingDto();
            List<ClassMeetings> classMeetings = new List<ClassMeetings>();
            return await _context.CLASS_MEETINGS.Where(m => m.CourseAllocation.CourseId == courseId && m.CourseAllocation.SessionSemester.Active && m.Date.Day >= DateTime.Now.Day)
                 .Include(c => c.CourseAllocation)
                 .ThenInclude(s => s.Course)
                 .Select(f => new MeetingDto
                 {
                     Topic = f.Topic,
                     Agenda = f.Agenda,
                     Date = f.Date,
                     Duration = f.Duration,
                     Time = f.Time,
                     Join_Meeting_Url = f.Join_Meeting_Url,
                     CourseId = f.CourseAllocation.CourseId,
                     CourseName = f.CourseAllocation.Course != null ? f.CourseAllocation.Course.CourseTitle : "-",
                     UserId = f.UserId

                 }).ToListAsync();


        }

        public async Task<bool> RefreshToken(MeetingDto meetingDto)
        {
            var rr = "https://zoom.us/oauth/token";
            var isRefreshed = false;
            RestClient restClient = new RestClient();
            var token = JObject.Parse(System.IO.File.ReadAllText(tokenFilePath));
            var request = new RestRequest();
            restClient.BaseUrl = new Uri(rr);
            request.AddQueryParameter("grant_type", "refresh_token");
            request.AddQueryParameter("refresh_token", token["refresh_token"].ToString());
            request.AddHeader("Authorization", "Basic " + AuthorizationHeader);

            var response = restClient.Post(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(tokenFilePath, response.Content);

                token = JObject.Parse(System.IO.File.ReadAllText(tokenFilePath));
                var userDetails = JObject.Parse(System.IO.File.ReadAllText(userTokenFilePath));
                var access_token = token["access_token"];
                var userId = userDetails["id"];

                var meetingModel = new JObject();
                meetingModel["topic"] = meetingDto.Topic;
                meetingModel["agenda"] = meetingDto.Agenda;
                meetingModel["start_time"] = meetingDto.Date.ToString("yyyy-mm-dd") + "T" + TimeSpan.FromHours(meetingDto.Time).ToString();
                meetingModel["duration"] = meetingDto.Duration;

                var meeting_url = $"https://api.zoom.us/v2/users/{userId}/meetings";

                var model = JsonConvert.SerializeObject(meetingModel);
                RestRequest restRequest = new RestRequest();
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddHeader("Authorization", string.Format("Bearer {0}", access_token));
                restRequest.AddParameter("application/json", model, ParameterType.RequestBody);

                restClient.BaseUrl = new Uri(meeting_url);
                response = restClient.Post(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    ClassMeetings classMeetings = new ClassMeetings();
                    var courseExists = await _context.COURSE.Where(c => c.Id == meetingDto.CourseId).FirstOrDefaultAsync();
                    var userExists = await _context.USER.Where(c => c.Id == meetingDto.UserId).FirstOrDefaultAsync();

                    if ((courseExists != null && courseExists.Id > 0) && (userExists != null && userExists.Id > 0))
                    {
                        var json_response = JObject.Parse(response.Content);
                        var join_url = json_response["join_url"].ToString();
                        var start_url = json_response["start_url"].ToString();

                        classMeetings.Topic = meetingDto.Topic;
                        classMeetings.Date = meetingDto.Date;
                        classMeetings.Duration = meetingDto.Duration;
                        classMeetings.Time = meetingDto.Time;
                        classMeetings.Agenda = meetingDto.Agenda;
                        classMeetings.UserId = meetingDto.UserId;
                        classMeetings.CourseAllocationId = meetingDto.CourseAllocationId;
                        classMeetings.Start_Meeting_Url = start_url;
                        classMeetings.Join_Meeting_Url = join_url;
                        _context.Add(classMeetings);
                        await _context.SaveChangesAsync();
                    }

                    System.IO.File.WriteAllText(meetingRsponsePath, response.Content);
                    isRefreshed = true;
                }
                return isRefreshed;
                //return token["access_token"].ToString();

            }
            return isRefreshed;


        }


        public async Task<IEnumerable<MeetingDto>> GetLiveLecturesByInstructor(long InstructorId)
        {
            return await _context.CLASS_MEETINGS.Where(c => c.UserId == InstructorId)
                .Select(c => new MeetingDto
                {
                    CourseName = c.CourseAllocation.Course.CourseTitle,
                    CourseId = c.CourseAllocation.CourseId,
                    Start_Meeting_Url = c.Start_Meeting_Url,
                    Join_Meeting_Url = c.Join_Meeting_Url,
                    Date = c.Date,
                    Time = c.Time,
                    Topic = c.Topic,
                    Agenda = c.Agenda,
                    Duration = c.Duration,
                    UserId = c.UserId,
                    LiveLectureId = c.Id,
                    StartTime = c.StartTime

                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MeetingDto>> GetAllLiveLectures()
        {
            return await _context.CLASS_MEETINGS
                .Select(c => new MeetingDto
                {
                    CourseName = c.CourseAllocation.Course.CourseTitle,
                    CourseId = c.CourseAllocation.CourseId,
                    Start_Meeting_Url = c.Start_Meeting_Url,
                    Join_Meeting_Url = c.Join_Meeting_Url,
                    Date = c.Date,
                    Time = c.Time,
                    Topic = c.Topic,
                    Agenda = c.Agenda,
                    Duration = c.Duration,
                    UserId = c.UserId,
                    LiveLectureId = c.Id
                    

                })
                .ToListAsync();
        }
        public async Task<int> DeleteLiveLecture(long LiveLectureId)
        {
            try
            {
                var getItem = await _context.CLASS_MEETINGS.Where(c => c.Id == LiveLectureId).FirstOrDefaultAsync();
                _context.Remove(getItem);
                await _context.SaveChangesAsync();
                return StatusCodes.Status200OK;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ToggleClassOnlineStatus(long liveLectureId, bool status)
        {
            try
            {
                var getItem = await _context.CLASS_MEETINGS.Where(c => c.Id == liveLectureId).FirstOrDefaultAsync();
                getItem.IsLive = status;
                 _context.Update(getItem);
                await _context.SaveChangesAsync();
                return StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<MeetingDto>> GetActiveLiveLectures()
        {
            return await _context.CLASS_MEETINGS.Where(x => x.IsLive == true)
                .Select(c => new MeetingDto
                {
                    CourseName = c.CourseAllocation.Course.CourseTitle,
                    CourseId = c.CourseAllocation.CourseId,
                    Start_Meeting_Url = c.Start_Meeting_Url,
                    Join_Meeting_Url = c.Join_Meeting_Url,
                    Date = c.Date,
                    Time = c.Time,
                    Topic = c.Topic,
                    Agenda = c.Agenda,
                    Duration = c.Duration,
                    UserId = c.UserId,
                    LiveLectureId = c.Id


                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MeetingDto>> GetActiveLiveLecturesByDepartment(long departmentId)
        {
            List<MeetingDto> returnList = new List<MeetingDto>();
            var getInstructors = await _context.INSTRUCTOR_DEPARTMENT.Where(x => x.DepartmentId == departmentId && x.CourseAllocation.SessionSemester.Active).ToListAsync();
            if (getInstructors.Any())
            {
                for(var i = 0; i < getInstructors.Count(); i++)
                {
                    var activeLiveClasses = await _context.CLASS_MEETINGS.Where(x => x.IsLive == true && x.UserId == getInstructors[i].UserId)
                        .Select(c => new MeetingDto
                        {
                            CourseName = c.CourseAllocation.Course.CourseTitle,
                            CourseId = c.CourseAllocation.CourseId,
                            Start_Meeting_Url = c.Start_Meeting_Url,
                            Join_Meeting_Url = c.Join_Meeting_Url,
                            Date = c.Date,
                            Time = c.Time,
                            Topic = c.Topic,
                            Agenda = c.Agenda,
                            Duration = c.Duration,
                            UserId = c.UserId,
                            LiveLectureId = c.Id
                        })
                        .ToListAsync();
                    if (activeLiveClasses.Any())
                    {
                        returnList.AddRange(activeLiveClasses);
                    }
                }
            }
            return returnList;
        }
    }
}
