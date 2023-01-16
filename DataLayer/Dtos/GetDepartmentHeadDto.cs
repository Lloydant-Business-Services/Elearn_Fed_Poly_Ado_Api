using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetDepartmentHeadDto
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string HodName { get; set; }
        public string Email { get; set; }
    }
}
