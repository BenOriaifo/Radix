using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class NotificationSubscription
    {
        public NotificationSubscription()
        {
            NotificationPreferrences = new HashSet<NotificationPreferrence>();
        }

        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Pin { get; set; }

        public ICollection<NotificationPreferrence> NotificationPreferrences { get; set; }
    }
}
