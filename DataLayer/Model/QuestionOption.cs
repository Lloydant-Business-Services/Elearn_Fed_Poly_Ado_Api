using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class QuestionOption:BaseModel
    {
        public long ObjectiveExaminationId { get; set; }
        public long AnswerOptionsId { get; set; }
        public virtual ObjectiveExamination ObjectiveExamination { get; set; }
        public virtual AnswerOptions AnswerOptions { get; set; }
        public string OptionName { get; set; }
        public bool Active { get; set; }
    }
}
