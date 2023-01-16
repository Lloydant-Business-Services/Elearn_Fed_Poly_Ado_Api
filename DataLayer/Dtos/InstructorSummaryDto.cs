using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class InstructorSummaryDto
    {
        public long Courses { get; set; }
        public long Students { get; set; }
        public long Assignments { get; set; }
    }
}
