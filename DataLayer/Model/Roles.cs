using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public enum Roles
    {
        SchoolAdmin = 2,
        Instructor = 3,
        DepartmentAdministrator = 4,
        Student = 5,
        SubAdmin = 6
    }
     public enum TaskDelegations
    {
        School_dept_setup = 1,
        hod_management,
        instructor_management,
        student_management,
        session_semester_setup,
        report_admin
    }
}
