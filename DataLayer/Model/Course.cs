using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class Course : BaseModel
    {
        [MaxLength(50)]
        public string CourseCode { get; set; }
        [MaxLength(50)]
        public string CourseCodeSlug { get; set; }
        [MaxLength(100)]
        public string CourseTitle { get; set; }
        [MaxLength(100)]
        public string CourseTitleSlug { get; set; }
        public long UserId { get; set; }
        public long? LevelId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual User User { get; set; }
        public virtual Level Level { get; set; }
        public bool Active { get; set; }
    }
}
