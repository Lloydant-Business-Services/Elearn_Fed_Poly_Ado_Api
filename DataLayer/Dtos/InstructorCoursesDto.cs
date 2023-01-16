using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class InstructorCoursesDto
    {
        public long CourseAllocationId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
        public long CourseId { get; set; }
        public string Level { get; set; }
        public long RegisteredStudents { get; set; }
    }
}
