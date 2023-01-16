using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class Department : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public long FacultySchoolId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual FacultySchool FacultySchool { get; set; }
        public string slug { get; set; }
        public bool Active { get; set; }
    }
}
