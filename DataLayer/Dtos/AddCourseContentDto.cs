using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddCourseContentDto
    {
        public long CourseTopicId { get; set; }
        public IFormFile Note { get; set; }
        public string VideoLink { get; set; }
        public string StreamLink { get; set; }
        public string ContentTitle { get; set; }
    }
}
