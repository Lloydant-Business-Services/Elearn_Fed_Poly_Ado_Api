using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class Examination : BaseModel
    {
        public long CourseAllocationId { get; set; }
        public virtual CourseAllocation CourseAllocation { get; set; }
        [MaxLength(500)]
        public string ExamName { get; set; }
        public bool Active { get; set; }
        public int TimeAllowed { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
    }
}
