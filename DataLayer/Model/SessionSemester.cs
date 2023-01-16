using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class SessionSemester : BaseModel
    {
        public long SessionId { get; set; }
        public long SemesterId { get; set; }
        public bool Active { get; set; }
        public virtual Session Session { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
