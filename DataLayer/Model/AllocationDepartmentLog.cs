using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class AllocationDepartmentLog : BaseModel
    {
        public long CourseAllocationId { get; set; }  
        public long? DepartmentId { get; set; }
        public CourseAllocation CourseAllocation { get; set; }
        public Department Department { get; set; }
    }
}
