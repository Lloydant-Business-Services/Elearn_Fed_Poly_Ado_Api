using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class FacultySchool:BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string slug { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}
