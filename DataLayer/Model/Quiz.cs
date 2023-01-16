using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class Quiz : BaseModel
    {
        public string QuizName { get; set; }
        public string QuizInstruction { get; set; }
        public string QuizInText { get; set; }
        public string QuizVideoLink { get; set; }
        public string QuizUploadLink { get; set; }
        public DateTime SetDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal MaxScore { get; set; }
        public bool PublishResult { get; set; }
        public bool Active { get; set; }
        public bool IsDelete { get; set; }
        public long CourseAllocationId { get; set; }
        public long? CharacterLimit { get; set; }
        public CourseAllocation CourseAllocation { get; set; }
    }
}
