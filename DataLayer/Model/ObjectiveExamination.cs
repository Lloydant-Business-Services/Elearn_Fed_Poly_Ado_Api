using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Model
{
    public class ObjectiveExamination : BaseModel
    {
        public long ExaminationId { get; set; }
        public virtual Examination Examination { get; set; }
        public string Question { get; set; }
        public bool Active { get; set; }
        [Column("CorrectAnswer")]
        public long AnswerOptionsId { get; set; }
        public virtual AnswerOptions AnswerOptions { get; set; }
        public int Points { get; set; }
        public string QuestionImageUrl { get; set; }
    }
}
