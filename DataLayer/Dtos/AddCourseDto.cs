using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddCourseDto : BaseModel
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public long UserId { get; set; }
        public long? LevelId { get; set; }
        public long CourseAllocationId { get; set; }
        public DateTime DateCreated { get; set; }
    }


    public class CourseUploadModel
    {
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
    }
}
