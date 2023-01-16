using BusinessLayer.Interface;
using DataLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {

        private readonly IAnnouncementService _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public AnnouncementController(IAnnouncementService service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }

        [HttpPost("[action]")]
        public async Task<int> AddAnnouncement(AddAnnouncementDto dto) => await _service.AddAnnouncement(dto);
        [HttpGet("[action]")]
        public async Task<IEnumerable<GetAnnouncementDto>> GetAnnouncement(long departmentId) => await _service.GetAnnouncement(departmentId);

    }
}
