using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class CourseRegistration : BaseModel
    {
        public long StudentPersonId { get; set; }
        public long SessionSemesterId { get; set; }
        public long CourseAllocationId { get; set; }
        public DateTime DateRegistered { get; set; }
        public virtual StudentPerson StudentPerson { get; set; }
        public virtual CourseAllocation CourseAllocation { get; set; }
        public virtual SessionSemester SessionSemester { get; set; }
        public bool Active { get; set; }

    }
}
