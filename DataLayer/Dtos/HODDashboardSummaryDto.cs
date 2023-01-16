using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class HODDashboardSummaryDto
    {
        public long AllDepartmentInstructors { get; set; }
        public long AllDepartmentStudents { get; set; }
        public long AllDepartmentCourses { get; set; }
        public long AllDepartmentCourseMaterials { get; set; }
    }
}
