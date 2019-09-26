using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class MessageTemplate
    {
        public int Id { get; set; }
        public long MessageTypeId { get; set; }
        public string SmsTemplate { get; set; }
        public string EmailTemplate { get; set; }

        public MessageType MessageType { get; set; }
    }
}
