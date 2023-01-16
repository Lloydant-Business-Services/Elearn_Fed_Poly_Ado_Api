using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class User : BaseModel
    {
        [MaxLength(50)]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsVerified { get; set; }
        public bool? IsPasswordUpdated { get; set; }
        public string Guid { get; set; }
        [MaxLength(50)]
        //public string SecurityAnswer { get; set; }
        public DateTime SignUpDate { get; set; }
        public long RoleId { get; set; }
        //public long? SecurityQuestionId { get; set; }
        public long PersonId { get; set; }
        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
        //public virtual SecurityQuestion SecurityQuestion { get; set; }
        public bool Active { get; set; }

    }
    public class SubAdmin : BaseModel
    {
        public long UserId { get; set; }
        public bool Active { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
    }
    public class AdminDelegations : BaseModel
    {
        public long SubAdminId { get; set; }
        public TaskDelegations TaskId { get; set; }
        public bool Active { get; set; }
        public DateTime DateAdded { get; set; }
        public SubAdmin SubAdmin { get; set; }
    }
}
