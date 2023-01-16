using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetNotificationTrackerDto
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public EmailNotificationCategory EmailNotificationCategory { get; set; }
        public string NotificationDescription { get; set; }
        public string TItle { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public string DateAdded { get; set; }
    }
}
