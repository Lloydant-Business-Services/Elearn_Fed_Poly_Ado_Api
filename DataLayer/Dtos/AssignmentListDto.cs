using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AssignmentListDto
    {
        public long AssignmentId { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string AssignmentName { get; set; }
        public string InstructorName { get; set; }
        public decimal MaxScore { get; set; }
        public bool IsPublished { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime DueDate { get; set; }
        public bool Active { get; set; }
        public long? MaxCharacters { get; set; }

    }

    public class QuizListDto
    {
        public long QuizId { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string QuizName { get; set; }
        public string InstructorName { get; set; }
        public decimal MaxScore { get; set; }
        public bool IsPublished { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime DueDate { get; set; }
        public bool Active { get; set; }
        public long? MaxCharacters { get; set; }

    }
}
