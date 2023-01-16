using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Enums
{
    public enum SystemRole
    {
        Admin = 2,
        Instructor,
        HOD,
        Student,
        SubAdmin,
        ServiceAdmin
    }

    public enum PaymentCheck
    {
        EnabledAndPaid = 1,
        EnabledAndNotPaid,
        Disabled
    }
}
