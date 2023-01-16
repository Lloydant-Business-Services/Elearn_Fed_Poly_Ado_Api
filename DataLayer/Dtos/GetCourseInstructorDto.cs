using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetCourseInstructorDto
    {
        public string Fullname { get; set; }
        public long UserId { get; set; }
        public long LecturerDepartmentId { get; set; }
    }
}
