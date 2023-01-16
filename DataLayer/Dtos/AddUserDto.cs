using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddUserDto
    {
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public string Surname { get; set; }
        //public string Username { get; set; }
        public string Email { get; set; }
        //public long CourseAllocationId { get; set; }
        public long DepartmentId { get; set; }
        //public long UserId { get; set; }
        public long RoleId { get; set; }
        public int Task { get; set; }
        //public string FullName { get; set; }
    }
}
