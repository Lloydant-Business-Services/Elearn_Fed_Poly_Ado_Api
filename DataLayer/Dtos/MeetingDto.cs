using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class MeetingDto
    {
        public string Topic { get; set; }
        public string Agenda { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }
        public long CourseId { get; set; }
        public long CourseAllocationId { get; set; }
        public long UserId { get; set; }
        public long LiveLectureId { get; set; }
        public string Start_Meeting_Url { get; set; }
        public string Join_Meeting_Url { get; set; }
        public string CourseName { get; set; }
    }
}
