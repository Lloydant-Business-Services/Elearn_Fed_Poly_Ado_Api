using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddAnnouncementDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public long? DepartmentId { get; set; }
        public bool Active { get; set; }
    }
}
