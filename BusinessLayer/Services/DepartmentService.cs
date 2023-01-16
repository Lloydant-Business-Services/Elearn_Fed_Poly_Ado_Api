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
    public class DepartmentService : Repository<Department>, IDepartmentService
    {

        private readonly IConfiguration _configuration;
        public readonly string baseUrl;
        ResponseModel response = new ResponseModel();

        public DepartmentService(ELearnContext context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            baseUrl = configuration.GetValue<string>("Url:root");

        }


        public async Task<ResponseModel> AddDepartment(DepartmentDto model)
        {
            try
            {
                Department dept = new Department();
                var d_slug = Utility.GenerateSlug(model.Name);
                //var doesExist = await _context.DEPARTMENT.Where(f => f.slug == d_slug).FirstOrDefaultAsync();
                //if (doesExist != null)
                //{
                //    response.StatusCode = StatusCodes.Status208AlreadyReported;
                //    response.Message = "Faculty/School Already Added";
                //    return response;
                //}
                dept.Name = model.Name;
                dept.slug = d_slug;
                dept.FacultySchoolId = model.FacultyId;
                dept.Active = true;
                dept.DateCreated = DateTime.Now;
                _context.Add(dept);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel> UpdateDepartment(DepartmentDto model)
        {
            try
            {
                Department dept = await _context.DEPARTMENT.Where(f => f.Id == model.Id).FirstOrDefaultAsync();
                if (dept == null)
                    throw new NullReferenceException("Department not found");
                var d_slug = Utility.GenerateSlug(model.Name);
                dept.Name = model.Name;
                dept.slug = d_slug;
                if(model.FacultyId > 0)
                {
                    dept.FacultySchoolId = model.FacultyId;
                }
                dept.Active = true;
                _context.Update(dept);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseModel> DeleteDepartment(long id)
        {
            try
            {
                Department dept = await _context.DEPARTMENT.Where(f => f.Id == id).FirstOrDefaultAsync();
                if (dept != null)
                {
                    dept.Active = dept.Active ? false : true;
                    _context.Update(dept);
                    await _context.SaveChangesAsync();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Message = "deleted";
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseModel> AssignDepartmentHead(AddDepartmentHeadDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                var doesExist = await _context.DEPARTMENT_HEADS.Where(d => d.DepartmentId == dto.DepartmentId && d.Active).FirstOrDefaultAsync();
                if (doesExist != null)
                {
                    response.Message = "HOD already assigned";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;

                }
                DepartmentHeads departmentHeads = new DepartmentHeads()
                {
                    DepartmentId = dto.DepartmentId,
                    UserId = dto.UserId,
                    Active = true
                };
                _context.Add(departmentHeads);
                await _context.SaveChangesAsync();
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<GetDepartmentHeadDto>> GetDepartmentHeadsByFaculty(long facultyId)
        {
            return await _context.DEPARTMENT_HEADS.Where(a => a.Active && a.Department.FacultySchool.Id == facultyId)
                .Include(u => u.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Select(f => new GetDepartmentHeadDto
                {
                    UserId = f.UserId,
                    DepartmentId = f.DepartmentId,
                    DepartmentName = f.Department.Name,
                    HodName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
                    Email = f.User.Username
                })
                .OrderBy(d => d.HodName)
                .ToListAsync();
        }
        public async Task<GetDepartmentHeadDto> GetDepartmentHeadByDepartmentId(long departmentId)
        {
            return await _context.DEPARTMENT_HEADS.Where(a => a.Active && a.DepartmentId == departmentId)
                .Include(u => u.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Select(f => new GetDepartmentHeadDto
                {
                    UserId = f.UserId,
                    DepartmentId = f.DepartmentId,
                    DepartmentName = f.Department.Name,
                    HodName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername
                })
                .OrderBy(d => d.HodName)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<GetDepartmentHeadDto>> GetAllDepartmentHeads()
        {
            return await _context.DEPARTMENT_HEADS.Where(a => a.Active)
                .Include(u => u.User)
                .ThenInclude(p => p.Person)
                .Include(d => d.Department)
                .ThenInclude(f => f.FacultySchool)
                .Select(f => new GetDepartmentHeadDto
                {
                    UserId = f.UserId,
                    DepartmentId = f.DepartmentId,
                    DepartmentName = f.Department.Name,
                    HodName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername
                })
                .OrderBy(d => d.HodName)
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

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByFacultyId(long facultyId, bool isAdmin)
        {

            if (isAdmin)
            {
                return await _context.DEPARTMENT.Where(d => d.FacultySchoolId == facultyId && d.Active)
                .Select(d => new DepartmentDto
                {
                    Name = d.Name,
                    Id = d.Id,
                    DateCreated = d.DateCreated,
                    Active = d.Active
                })
                .OrderBy(d => d.Name)
                .ToListAsync();
            }
            else
            {
                return await _context.DEPARTMENT.Where(d => d.FacultySchoolId == facultyId && d.Active)
                .Select(d => new DepartmentDto
                {
                    Name = d.Name,
                    Id = d.Id,
                    DateCreated = d.DateCreated,
                    Active = d.Active
                })
                .OrderBy(d => d.Name)
                .ToListAsync();
            }
            
                
        }
    }
}
