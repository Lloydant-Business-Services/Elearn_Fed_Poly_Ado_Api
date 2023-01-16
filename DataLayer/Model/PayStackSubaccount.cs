using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class PayStackSubaccount
    {
        public string business_name { get; set; }
        public string settlement_bank { get; set; }
        public string account_number { get; set; }
        public float percentage_charge { get; set; }
        public string primary_contact_email { get; set; }
        public string primary_contact_name { get; set; }
        public string primary_contact_phone { get; set; }
        public string settlement_schedule { get; set; }
    }
}
