using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IRepository<Session> _repo;
        private readonly ELearnContext _context;

        public SessionController(IRepository<Session> repo, ELearnContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<long> AddSesion([FromBody] Session session) => await _repo.Insert(session);
        [HttpGet]
        public async Task<IEnumerable<Session>> GetAll()
        {
            return await _context.SESSION.Where(x => x.Active)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public Session GetById(long id) => _repo.GetById(id);

        [HttpPost]
        public void Delete(long id) => _repo.Delete(id);
        [HttpPost("[action]")]
        public async Task<bool> ModifySession(long sessionId, string sessionName)
        {
            var getSession = await _context.SESSION.Where(x => x.Id ==sessionId).FirstOrDefaultAsync();
            if (getSession != null)
            {
                getSession.Name = sessionName;
                _context.Update(getSession);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
