using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetInstructorDto
    {
        //public long PersonId { get; set; }
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public string FullName { get; set; }
        public string Level { get; set; }
        public string Email { get; set; }
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
        public Department Department { get; set; }
    }
}
