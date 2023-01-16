using DataLayer.Enums;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public long PersonId { get; set; }
        public long UserId { get; set; }
        public List<TaskDelegations> Delegations { get; set; }
        public string FullName { get; set; }
        public bool IsHOD { get; set; }
        public bool IsSubAdmin { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? IsPasswordUpdated { get; set; }
        public PaymentCheck PaymentCheck { get; set; }

    }
}
