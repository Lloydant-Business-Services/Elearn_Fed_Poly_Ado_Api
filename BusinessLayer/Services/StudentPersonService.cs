using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class StudentPersonService : Repository<StudentPerson>, IStudentPersonService
    {
        //private readonly ELearnContext _context;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly string defaultPassword = "1234567";

        public StudentPersonService(IConfiguration configuration, ELearnContext context)
            : base(context)
        {
            //_context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

       public async Task<StudentPerson> GetStudentPersonBy(string MatricNo)
        {
            try
            {
                var matNoSlug = Utility.GenerateSlug(MatricNo);
                return await _context.STUDENT_PERSON.Where(x => x.MatricNoSlug == matNoSlug).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
