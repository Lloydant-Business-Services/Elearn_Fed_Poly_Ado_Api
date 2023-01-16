using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RWLogController : ControllerBase
    {
        private readonly ELearnContext _context;

        public RWLogController(ELearnContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<bool> AddLog(string name, string category, int amount)
        {
            RwLog rwLog = new RwLog()
            {
                Name = name,
                Amount = amount,
                Category = category
            };
            _context.Add(rwLog);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<RwLog>> GetAllLog()
        {
            return await _context.RWLOG.ToListAsync();
        }

        [HttpPost("[action]")]
        public async Task<bool> DeleteLog(long id)
        {
            var getLog = await _context.RWLOG.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Remove(getLog);
            await _context.SaveChangesAsync();
            return true;

        }

        [HttpPost("[action]")]
        public async Task<bool> UpdateLog(long id, string name, int amount)
        {
            var getLog = await _context.RWLOG.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(getLog != null)
            {
                getLog.Amount = amount > 0 ? amount : getLog.Amount;
                getLog.Name = name != null ? name : getLog.Name;
            }
            _context.Update(getLog);
            await _context.SaveChangesAsync();
            return true;

        }

        //ratings

        [HttpPost("[action]")]
        public async Task<bool> AddRatings(string name, int rates, string comments, string table)
        {
            Ratings rwLog = new Ratings()
            {
                Name = name,
                Rates = rates,
                Comments = comments,
                Table = table,
            };
            _context.Add(rwLog);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<Ratings>> GetAllRatings()
        {
            return await _context.RATINGS.ToListAsync();
        }

        [HttpPost("[action]")]
        public async Task<bool> DeleteRatings(long id)
        {
            var getLog = await _context.RATINGS.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Remove(getLog);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
