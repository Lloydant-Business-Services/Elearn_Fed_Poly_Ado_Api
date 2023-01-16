using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GradeAssignmentDto
    {
        public long AssignmentSubmissionId { get; set; }
        public decimal Score { get; set; }
        public string Remark { get; set; }
    }

    public class GradeQuizDto
    {
        public long QuizSubmissionId { get; set; }
        public decimal Score { get; set; }
        public string Remark { get; set; }
    }
}
