using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AssignmentCumulativeDto
    {
       // public long AssignmentSubmissionId { get; set; }
        //public long AssignmentId { get; set; }
        public decimal CumulativeScore { get; set; }
        public string StudentName { get; set; }
        public string MatricNumber { get; set; }
    }
}
