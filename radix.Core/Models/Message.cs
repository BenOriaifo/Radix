using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class Message
    {
        public long Id { get; set; }
        public string MessageCode { get; set; }
        public DateTime? DateExtracted { get; set; }
        public string RSAPIN { get; set; }
        public string RSAName { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string SmsMessage { get; set; }
        public string EmailMessage { get; set; }
        public long? IsSent { get; set; }
        public DateTime? DateSent { get; set; }
        public string Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MessageId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerCode { get; set; }
    }
}
