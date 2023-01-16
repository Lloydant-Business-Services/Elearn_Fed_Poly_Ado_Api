using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetCourseTopicDto
    {
        public long CourseId { get; set; }
        public string TopicName { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string CourseCode { get; set; }
        //public string CourseAccessCode { get; set; }
        public string CourseTitle { get; set; }
        public string TopicDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime SetDate { get; set; }
        public long TopicId { get; set; }
    }
}
