using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ChangePasswordDto
    {
        public long UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
