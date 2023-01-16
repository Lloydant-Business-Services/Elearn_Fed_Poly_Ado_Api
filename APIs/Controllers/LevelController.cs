using APIs.Middleware;
using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly IRepository<Level> _repo;
        private readonly ELearnContext _context;
        public LevelController(IRepository<Level> repo, ELearnContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("GetLevels")]
        public IEnumerable<Level> Levels() => _repo.GetAll();
        
        
        
        [HttpPost("PostLevel")]
        public async Task<int> AddLevel(Level level)
        {
            Level addLevel = new Level()
            {
                Name = level.Name,
                Active = true
            };
            _context.Add(addLevel);
            await _context.SaveChangesAsync();
            return StatusCodes.Status200OK;
        }

        [HttpDelete("DeleteLevel")]
        public async Task<int> RemoveLevel(long id)
        {
            var level = await _context.LEVEL.Where(d => d.Id == id).FirstOrDefaultAsync();
            if(level != null)
            {
                _context.Remove(level);
                await _context.SaveChangesAsync();
                return StatusCodes.Status200OK;
            }
            throw new NullReferenceException("Not found");
        }
    }
}
