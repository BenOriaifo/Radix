using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radix.ViewModel
{
    public partial class MessageViewModel
    {
        public long Id { get; set; }
        public string MessageCode { get; set; }
        [Required]
        public string RSAPIN { get; set; }
        [Required]
        public string FullName { get; set; }
        public string MobilePhone { get; set; }

        public string SmsMessage { get; set; }
        public string EmailMessage { get; set; }

        public long? IsSent { get; set; }
        public DateTime? DateSent { get; set; }
        public string Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MessageId { get; set; }
        public string Email { get; set; }
        public DateTime? DateExtracted { get; set; }
    }
}
