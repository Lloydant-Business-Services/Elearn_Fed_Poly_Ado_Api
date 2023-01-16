using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class CourseTopic : BaseModel
    {
        [MaxLength(200)]
        public string Topic { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public long CourseAllocationId { get; set; }
        public DateTime SetDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public bool IsArchieved { get; set; }
        public virtual CourseAllocation CourseAllocation { get; set; }
    }
}
