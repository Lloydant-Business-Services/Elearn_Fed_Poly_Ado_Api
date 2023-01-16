using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultySchoolController : ControllerBase
    {
        private readonly IFacultyService _service;
        private readonly ELearnContext _context;
        public FacultySchoolController(IFacultyService service, ELearnContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<ResponseModel> AddFacultySchool(FacultyDto model) => await _service.AddFacultySchool(model);
        //public async Task<long> AddFacultySchool([FromBody] FacultySchool facultySchool) => await _service.Insert(facultySchool);

        [HttpGet("[action]")]
        public async Task<IEnumerable<FacultyDto>> GetFaculties(bool isAdmin)
        {
            if (isAdmin)
            {
                return await _context.FACULTY_SCHOOL.Where(a => a.Active)
                .Select(f => new FacultyDto
                {
                    Name = f.Name,
                    Id = f.Id,
                    Active = f.Active,
                    DateCreated = f.DateCreated
                })
                .OrderBy(a => a.Name)
                .ToListAsync();
            }
            else
            {
                return await _context.FACULTY_SCHOOL.Where(a => a.Active)
                .Select(f => new FacultyDto
                {
                    Name = f.Name,
                    Id = f.Id,
                    Active = f.Active,
                    DateCreated = f.DateCreated
                }).ToListAsync();
            }
            
                
        }
        [HttpGet("{id}")]
        public FacultySchool GetBy(long id) => _service.GetById(id);
        [HttpPost("[action]")]
        public async Task<ResponseModel> UpdateFacultySchool(FacultyDto model) => await _service.UpdateFacultySchool(model);
        [HttpPost("[action]")]
        public async Task<ResponseModel> DeleteFacultySchool(long id) => await _service.DeleteFacultySchool(id);
    }
}
