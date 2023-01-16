using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class UpdateUserProfileDto
    {
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string PhoneNumber { get; set; }
        public long? DepartmentId { get; set; }
        public long GenderId { get; set; }
        //public bool IsUpdatedProfile { get; set; }
    }
}
