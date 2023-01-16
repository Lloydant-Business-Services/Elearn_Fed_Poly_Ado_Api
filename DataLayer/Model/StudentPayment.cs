using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Model
{
    public class StudentPayment : BaseModel
    {
        public long? SessionId { get; set; }
        public Session Session { get; set; }
        public int? Amount { get; set; }
        public bool? Active { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(20)]
        public string StatusCode { get; set; }
        public DateTime Created { get; set; }
        [StringLength(150)]
        public string ClientPortalIdentifier { get; set; }// eg invoice number
        public long? DepartmentId { get; set; }
        public Department Department { get; set; }
        public long? ProgrammeId { get; set; }
        [StringLength(20)]
        public string SystemCode { get; set; }
        [StringLength(100)]
        [Required]
        public string SystemPaymentReference { get; set; }
        public string PaymentGateway { get; set; }
        [Required]
        public long PersonId { get; set; }
        public Person Person { get; set; }
        public int? PaymentMode { get; set; }//eg card, bankteller,transfer
        public DateTime? DatePaid { get; set; }
        public bool? IsPaid { get; set; }

        public PaymentSetup PaymentSetup { get; set; }
        public long PaymentSetupId { get; set; }
        public SessionSemester SessionSemester { get; set; }
        public long SessionSemesterId { get; set; }


    }

}
