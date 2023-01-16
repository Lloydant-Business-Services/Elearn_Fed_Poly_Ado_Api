using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class CourseContent : BaseModel
    {
        public string Material { get; set; }
        public string Link { get; set; }
        public string LiveStream { get; set; }
        public long CourseTopicId { get; set; }
        public DateTime SetDate { get; set; }
        public bool Active { get; set; }
        public bool IsArchieved { get; set; }
        public string ContentTitle { get; set; }
        public virtual CourseTopic CourseTopic { get; set; }
    }
}
