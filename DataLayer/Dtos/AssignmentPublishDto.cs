using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AssignmentPublishDto
    {
        public long AssignmentId { get; set; }
        public bool Publish { get; set; }
    }
    public class QuizPublishDto
    {
        public long QuizId { get; set; }
        public bool Publish { get; set; }
    }
}
