using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddAssignmentDto
    {
        public string Name { get; set; }
        public string AssignmentInstruction { get; set; }
        public string AssignmentInText { get; set; }
        public string AssignmentVideoLink { get; set; }
        public IFormFile AssignmentUpload { get; set; }
        public DateTime DueDate { get; set; }
        public decimal MaxScore { get; set; }
        public long CourseAllocationId { get; set; }
        public long MaxCharacters { get; set; }
    }



    public class AddQuizDto
    {
        public string Name { get; set; }
        public string QuizInstruction { get; set; }
        public string QuizInText { get; set; }
        public string QuizVideoLink { get; set; }
        public IFormFile QuizUpload { get; set; }
        public DateTime DueDate { get; set; }
        public decimal MaxScore { get; set; }
        public long CourseAllocationId { get; set; }
        public long MaxCharacters { get; set; }

    }
}
