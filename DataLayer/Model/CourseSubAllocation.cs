using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class CourseSubAllocation : BaseModel
    {
        public long CourseAllocationId { get; set; }
        public virtual CourseAllocation CourseAllocation { get; set; }
        public long SubInstructorId { get; set; }
        public virtual User SubInstructor { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
