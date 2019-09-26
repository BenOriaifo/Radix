using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class MessageType
    {
        public MessageType()
        {
            MessageTemplate = new HashSet<MessageTemplate>();
            NotificationPreferrence = new HashSet<NotificationPreferrence>();
            ServiceConfiguration = new HashSet<ServiceConfiguration>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        public ICollection<MessageTemplate> MessageTemplate { get; set; }
        public ICollection<NotificationPreferrence> NotificationPreferrence { get; set; }
        public ICollection<ServiceConfiguration> ServiceConfiguration { get; set; }
    }
}
