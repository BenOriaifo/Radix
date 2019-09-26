using System;
using System.Collections.Generic;

namespace Radix.Data
{
    public partial class NotificationPreferrences
    {
        public long Id { get; set; }
        public long? NotificationSubscriptionId { get; set; }
        public int? MessageTypeId { get; set; }

        public MessageTypes MessageType { get; set; }
        public NotificationSubscriptions NotificationSubscription { get; set; }
    }
}
