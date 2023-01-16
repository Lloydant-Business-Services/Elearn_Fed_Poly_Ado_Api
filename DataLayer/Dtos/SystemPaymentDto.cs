using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class SystemPaymentDto
    {
        public long Id { get; set; }
        public string SystemPaymentReference { get; set; }
        public string SessionSemester { get; set; }
        public string Department { get; set; }
        public string Fullname { get; set; }
        public int? Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? Created { get; set; }



    }
}
