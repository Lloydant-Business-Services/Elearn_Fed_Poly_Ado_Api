using BusinessLayer.Interface;
using DataLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperPatchController : ControllerBase
    {
        private readonly IDeveloperPatchService _service;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public DeveloperPatchController(IDeveloperPatchService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");

        }
        [HttpPost("[action]")]
        public async Task<ExcelSheetUploadAggregation> ProcessAPIData(IEnumerable<StudentUploadModel> studentList, long departmentId) => await _service.ProcessAPIData(studentList, departmentId);
        [HttpGet("[action]")]
        public async Task<IEnumerable<StudentUploadModel>> MockAPIData() => await _service.MockAPIData();
    }
}
