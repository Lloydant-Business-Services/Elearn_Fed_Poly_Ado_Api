using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class StudentPerson : BaseModel
    {
        [MaxLength(100)]
        public string MatricNo { get; set; }
        [MaxLength(100)]
        public string MatricNoSlug { get; set; }
        public long PersonId { get; set; }
        public long? DepartmentId { get; set; }
        public bool Active { get; set; }
        public virtual Person Person { get; set; }
        public virtual Department Department { get; set; }
    }
}
