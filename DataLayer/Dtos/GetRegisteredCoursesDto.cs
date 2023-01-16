using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetRegisteredCoursesDto
    {
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
        public string CourseLecturer { get; set; }
        public long CourseId { get; set; }
        public long CourseAllocationId { get; set; }
        public DateTime DateRegistered { get; set; }

    }
}
