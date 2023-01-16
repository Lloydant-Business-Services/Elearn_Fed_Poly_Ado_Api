using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class Semester :BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public bool Active { get; set; } = true;
        public int? SortOrder { get; set; }

    }
}
