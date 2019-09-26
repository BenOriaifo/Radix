using System;
using System.Collections.Generic;

namespace Radix.Data
{
    public partial class MessageTemplates
    {
        public int Id { get; set; }
        public int? MessageTypeId { get; set; }
        public string Smstemplate { get; set; }
        public string EmailTemplate { get; set; }

        public MessageTypes MessageType { get; set; }
    }
}
