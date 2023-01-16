using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionSemesterController : ControllerBase
    {
        private readonly ISessionSemesterService _service;
        private readonly ELearnContext _context;
        

        public SessionSemesterController(ISessionSemesterService service, ELearnContext context)
        {
            _service = service;
            _context = context;
        }
        [HttpPost("[action]")]
        public async Task<ResponseModel> SetSessionSemester(long sessionId, long semesterId, long userId) => await _service.SetSessionSemester(sessionId, semesterId, userId);
        [HttpGet("[action]")]
        public async Task<GetSessionSemesterDto> GetActiveSessionSemester() => await _service.GetActiveSessionSemester();
        [HttpGet("[action]")]
        public async Task<List<GetSessionSemesterDto>> GetALLSessionSemester() => await _service.GetALLSessionSemester();


    }
}
