using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class NotificationTracker : BaseModel
    {
        public Person Person { get; set; }
        public long PersonId { get; set; }
        public EmailNotificationCategory EmailNotificationCategory { get; set; }
        public string NotificationDescription { get; set; }
        public string TItle { get; set; }
        public bool? Active { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
