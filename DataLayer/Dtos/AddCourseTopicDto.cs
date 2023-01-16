using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddCourseTopicDto
    {
        public long CourseAllocationId { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
