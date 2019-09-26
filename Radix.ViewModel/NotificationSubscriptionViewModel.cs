using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radix.ViewModel
{
    public partial class NotificationSubscriptionViewModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        [Required]
        public string Pin { get; set; }

        public List<NotificationPreferrenceViewModel> NotificationPreferrences { set; get; }
    }
}
