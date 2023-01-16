using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class RegisterCourseDto
    {
        //public long SessionSemesterId { get; set; }
        public long PersonId { get; set; }
        public List<Allocation> CourseAllocation { get; set; }

    }
    public class Allocation
    {
        public long CourseAllocationId { get; set; }

    }

}
