using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class Announcement : BaseModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Department Department { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public long? DepartmentId { get; set; }
        public bool Active { get; set; }
    }
}
