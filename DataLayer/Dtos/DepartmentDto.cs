using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class DepartmentDto : BaseModel
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public long FacultyId { get; set; }
        public bool Active { get; set; }


    }

    public class FacultyDto : BaseModel
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

    }
}
