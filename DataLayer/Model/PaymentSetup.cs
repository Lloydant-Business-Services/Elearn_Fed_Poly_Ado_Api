using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class PaymentSetup : BaseModel
    {
        public string FeeName { get; set; }
        public int Amount { get; set; }
        public bool Active { get; set; }
    }
}
