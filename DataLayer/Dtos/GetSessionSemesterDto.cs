using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetSessionSemesterDto : BaseModel
    { 
        public long SessionId { get; set; }
        public long SemesterId { get; set; }
        public string SemesterName { get; set; }
        public string SessionName { get; set; }

    }
}
