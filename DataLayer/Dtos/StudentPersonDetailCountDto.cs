using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class StudentPersonDetailCountDto
    {
        public long CoursesRegistered { get; set; }
        public long AssignmentsAttempted { get; set; }
        public long QuizAttempted { get; set; }
        public long TotalAssignmentsAvailable { get; set; }
        public long TotalQuizAvailable { get; set; }
    }
}
