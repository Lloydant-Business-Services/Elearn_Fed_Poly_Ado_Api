using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class StudentExamObjectiveAnswer:BaseModel
    {
        public long StudentExaminationId { get; set; }
        public virtual StudentExamination StudentExamination { get; set; }
        public long QuestionOptionId { get; set; }
        public virtual QuestionOption QuestionOption { get; set; }
    }
}
