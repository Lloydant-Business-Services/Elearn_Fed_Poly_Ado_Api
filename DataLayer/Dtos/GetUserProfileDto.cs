using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetUserProfileDto : BaseModel
    {
        //public string Firstname { get; set; }
        //public string Othername { get; set; }
        //public string Surname { get; set; }
        public string Username { get; set; }
       // public string Email { get; set; }
        public long UserId { get; set; }
        public string RoleName { get; set; }
        //public string FullName { get; set; }
       // public string PhoneNumber { get; set; }
        public string MatricNumber { get; set; }
        public Department Department { get; set; }
        public Person Person { get; set; }
        //public long GenderId { get; set; }
        public bool IsUpdatedProfile { get; set; }
    }
}
