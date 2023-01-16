using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Model
{
    public class Otp_Code : BaseModel
    {
        public string Otp { get; set; }
        public long? UserId { get; set; }
        public OTPStatus OTPStatus { get; set; }
        public DateTime? DateAdded { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
