using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Enums
{
   
        public enum EmailNotificationCategory
        {
            OTP = 1,
            PasswordReset,
            LiveLectureAlert,
            AccountAdded
        }
        public enum EmailTemplate
        {
            OTP = 1,
            PasswordReset,
        }
        public enum OTPStatus
        {
            GENERATED = 1,
            USED,
            EXPIRED
        }

}
