using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddSubInstructorDto
    {   
            public long UserId { get; set; }
            public long CourseAllocationId { get; set; }
            public long Id { get; set; }
            public DateTime DateAdded { get; set; }
            //public bool Active { get; set; }

        public class GetSubInstructorDto
        {
            public long UserId { get; set; }
            public long CourseId { get; set; }
            public DateTime DateAdded { get; set; }
            public bool Active { get; set; }
            public string PersonName { get; set; }
            public long Id { get; set; }
            public string CourseName { get; set; }
        }
    }
}
