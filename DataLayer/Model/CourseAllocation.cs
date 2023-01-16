using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class CourseAllocation : BaseModel
    {
        public long CourseId { get; set; }
        public virtual Course Course { get; set; }
        public long CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public long InstructorId { get; set; }
        public virtual User Instructor { get; set; }
        public DateTime DateCreated { get; set; }
        public long SessionSemesterId { get; set; }
        public long? DepartmentId { get; set; }
        public virtual SessionSemester SessionSemester { get; set; }
        public virtual Department Department { get; set; }
        public virtual Level Level { get; set; }
        public long? LevelId { get; set; }
        public bool Active { get; set; }


    }
}
