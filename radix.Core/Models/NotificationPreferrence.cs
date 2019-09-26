using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class NotificationPreferrence
    {
        public long Id { get; set; }
        public long? NotificationSubscriptionId { get; set; }
        public long MessageTypeId { get; set; }

        public MessageType MessageType { get; set; }
        public NotificationSubscription NotificationSubscription { get; set; }
    }
}
