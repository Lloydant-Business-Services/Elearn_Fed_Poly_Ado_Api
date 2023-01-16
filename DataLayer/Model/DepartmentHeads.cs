using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class DepartmentHeads : BaseModel
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public bool Active { get; set; }
    }
}
