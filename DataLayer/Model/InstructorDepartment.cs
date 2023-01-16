using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class InstructorDepartment : BaseModel
    {
        public User User { get; set; }
        public CourseAllocation CourseAllocation { get; set; }
        public long? CourseAllocationId { get; set; }
        public Department Department { get; set; }
        public long DepartmentId { get; set; }
        public long UserId { get; set; }
        public bool Active { get; set; }

    }
}
