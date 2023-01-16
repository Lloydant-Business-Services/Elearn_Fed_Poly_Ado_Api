using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetDepartmentCourseDto
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string CourseLecturer { get; set; }
        public long InstructorUserId { get; set; }
        public long CourseId { get; set; }
        public long CourseAllocationId { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
