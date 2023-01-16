using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AssignmentSubmissionDto
    {
        public string AssignmentInTextSubmission { get; set; }
        public string AssignmentSubmissionUploadLink { get; set; }
        public string AssignmentSubmissionHostedLink { get; set; }
        public long AssignmentSubmissionId { get; set; }
        public long AssignmentId { get; set; }
        public string InstructorRemark { get; set; }
        public decimal Score { get; set; }
        public DateTime DateSubmitted { get; set; }
        public bool Active { get; set; }
        public string StudentName { get; set; }
        public string MatricNumber { get; set; }
        public bool IsPublished { get; set; }
        public bool? LateSubmission { get; set; }
        public long? MaxCharacters { get; set; }

    }

    public class QuizSubmissionDto
    {
        public string QuizInTextSubmission { get; set; }
        public string QuizSubmissionUploadLink { get; set; }
        public string QuizSubmissionHostedLink { get; set; }
        public long QuizSubmissionId { get; set; }
        public long QuizId { get; set; }
        public string InstructorRemark { get; set; }
        public decimal Score { get; set; }
        public DateTime DateSubmitted { get; set; }
        public bool Active { get; set; }
        public string StudentName { get; set; }
        public string MatricNumber { get; set; }
        public bool IsPublished { get; set; }
        public long? MaxCharacters { get; set; }

    }


}
