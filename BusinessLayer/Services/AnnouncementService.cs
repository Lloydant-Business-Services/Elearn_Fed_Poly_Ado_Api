using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;

        public AnnouncementService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }

     
        public async Task<int> AddAnnouncement(AddAnnouncementDto dto)
        {
            try
            {
                var getUser = await _context.USER.Where(x => x.Id == dto.UserId).FirstOrDefaultAsync();
                if (getUser == null)
                    throw new NullReferenceException("User not found");
                Announcement announcement = new Announcement();
                announcement.Title = dto.Title;
                announcement.Message = dto.Message;
                announcement.UserId = getUser.Id;
                if(dto.DepartmentId > 0)
                {
                    announcement.DepartmentId = dto.DepartmentId;
                }
                announcement.Active = true;
                _context.Add(announcement);
                await _context.SaveChangesAsync();
                return StatusCodes.Status200OK;               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GetAnnouncementDto>> GetAnnouncement(long departmentId)
        {
            Department department = new Department();
            if (departmentId > 0)
            {
                department = await _context.DEPARTMENT.Where(d => d.Id == departmentId).FirstOrDefaultAsync();

            }
            //if (department == null)
            //throw new NullReferenceException("department not found");
            return await _context.ANNOUNCEMENT.Where(x => (x.Active && x.DepartmentId == departmentId) || (x.Active && x.DepartmentId == null))
                .Select(f => new GetAnnouncementDto { 
                    Title = f.Title,
                    Message = f.Message,
                    Sender = f.User.Role.Id == 4 ? "H.O.D " + department.Name : "University Management",
                    UserId = f.UserId,
                    
                })
                .ToListAsync();
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
