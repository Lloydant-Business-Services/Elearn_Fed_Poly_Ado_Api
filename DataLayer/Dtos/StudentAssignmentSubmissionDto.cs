using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class StudentAssignmentSubmissionDto
    {
        public long StudentUserId { get; set; }
        public string AssignmentInText { get; set; }
        public string AssignmentHostedLink { get; set; }
        public IFormFile AssignmentUpload { get; set; }
        public long AssignmentId { get; set; }
    }

    public class StudentQuizSubmissionDto
    {
        public long StudentUserId { get; set; }
        public string QuizInText { get; set; }
        public string QuizHostedLink { get; set; }
        public IFormFile QuizUpload { get; set; }
        public long QuizId { get; set; }
    }
}
