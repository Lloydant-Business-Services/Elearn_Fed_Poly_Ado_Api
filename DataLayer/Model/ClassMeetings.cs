using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class ClassMeetings : BaseModel
    {
        [MaxLength(200)]
        public string Topic { get; set; }
        [MaxLength(200)]
        public string Agenda { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        [MaxLength(5)]
        public int Time { get; set; }
        [MaxLength(5)]
        public int Duration { get; set; }
        public long CourseAllocationId { get; set; }
        public virtual CourseAllocation CourseAllocation { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public string Start_Meeting_Url { get; set; }
        public string Join_Meeting_Url { get; set; }
        public bool? IsLive { get; set; }
    }
}
