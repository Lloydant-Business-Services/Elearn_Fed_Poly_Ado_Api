using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class StudentExamination:BaseModel
    {
        public long ExaminationId { get; set; }
        public virtual Examination Examination { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int TimeTaken { get; set; }
        public long StudentPersonId { get; set; }
        public virtual StudentPerson StudentPerson { get; set; }
        public string LinkToTheoryAnswer { get; set; }
    }
}
