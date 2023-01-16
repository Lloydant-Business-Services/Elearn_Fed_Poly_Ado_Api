using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CourseContentService : ICourseContentService
    {
        private readonly IConfiguration _configuration;
        private readonly ELearnContext _context;
        private readonly string defualtPassword = "1234567";

        public CourseContentService(IConfiguration configuration, ELearnContext context)
        {
            _configuration = configuration;
            _context = context;
        }
    }
}
