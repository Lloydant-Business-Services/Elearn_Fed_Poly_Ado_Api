using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ComprehensiveAssignmentCumulativeDto
    {
        public decimal CumulativeScore { get; set; }
        public string StudentName { get; set; }
        public string MatricNumber { get; set; }
        public List<DetailDto> DetailList { get; set; }
    }
    public class DetailDto
    {
        public string AssignmentName { get; set; }
        public decimal Score { get; set; }
    }


    //Student CUmulative Assignment report
    public class StudentCumulativeAssignmentModel
    {
        public decimal CumulativeScore { get; set; }
        public string StudentName { get; set; }
        public string MatricNumber { get; set; }
        public List<AssignmentCumulativeModel> StudentAssignmentModel { get; set; }
    }
    public class AssignmentCumulativeModel
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public decimal Score { get; set; }
    }
}
