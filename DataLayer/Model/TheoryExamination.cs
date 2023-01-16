using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class TheoryExamination:BaseModel
    {
        public virtual Examination Examination { get; set; }
        public long ExaminationId { get; set; }
        public string ImageUrl { get; set; }
        public string TheoryQuestionUrl { get; set; }
        public bool Active { get; set; }
    }
}
