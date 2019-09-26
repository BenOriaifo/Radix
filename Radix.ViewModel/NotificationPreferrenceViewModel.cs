using System;
using System.Collections.Generic;

namespace Radix.ViewModel
{
    public partial class NotificationPreferrenceViewModel
    {
        public long Id { get; set; }
        public long NotificationSubscriptionId { get; set; }
        public long MessageTypeId { get; set; }
    }
}
