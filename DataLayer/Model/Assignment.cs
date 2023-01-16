using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class Assignment : BaseModel
    {
        public string AssignmentName { get; set; }
        public string AssignmentInstruction { get; set; }
        public string AssignmentInText { get; set; }
        public string AssignmentVideoLink { get; set; }
        public string AssignmentUploadLink { get; set; }
        public DateTime SetDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal MaxScore { get; set; }
        public bool PublishResult { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public long CourseAllocationId { get; set; }
        public long? CharacterLimit { get; set; }
        public CourseAllocation CourseAllocation { get; set; }
        public long? MaxCharacters { get; set; }

    }
}
