using System;
using System.Collections.Generic;

namespace Radix.Data
{
    public partial class MessageTypes
    {
        public MessageTypes()
        {
            MessageTemplates = new HashSet<MessageTemplates>();
            NotificationPreferrences = new HashSet<NotificationPreferrences>();
            ServiceConfigurations = new HashSet<ServiceConfigurations>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        public ICollection<MessageTemplates> MessageTemplates { get; set; }
        public ICollection<NotificationPreferrences> NotificationPreferrences { get; set; }
        public ICollection<ServiceConfigurations> ServiceConfigurations { get; set; }
    }
}
