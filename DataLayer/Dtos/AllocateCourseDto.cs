using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AllocateCourseDto
    {
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public long InstructorId { get; set; }
        public long LevelId { get; set; }
        public long? DepartmentId { get; set; }
        //public long SessionSemesterId { get; set; }
    }
}
