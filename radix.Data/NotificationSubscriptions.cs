using System;
using System.Collections.Generic;

namespace Radix.Data
{
    public partial class NotificationSubscriptions
    {
        public NotificationSubscriptions()
        {
            NotificationPreferrences = new HashSet<NotificationPreferrences>();
        }

        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Pin { get; set; }

        public ICollection<NotificationPreferrences> NotificationPreferrences { get; set; }
    }
}
