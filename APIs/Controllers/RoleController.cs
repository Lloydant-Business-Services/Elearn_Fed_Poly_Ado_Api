using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRepository<Role> _repo;
        public RoleController(IRepository<Role> repo)
        {
            _repo = repo;
        }

        [HttpGet("GetRoles")]
        public IEnumerable<Role> AllRoles() => _repo.GetAll();
    }
}
