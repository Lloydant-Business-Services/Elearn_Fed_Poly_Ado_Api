using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class SubInstructorCourseDto : BaseModel
    {
        public long CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
    }
}
