using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radix.ViewModel
{
    public partial class AdhocMessageViewModel
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Smsmessage { get; set; }

        public string EmailMessage { get; set; }

        public bool? Status { get; set; }

        [Required]
        public string Recipient { get; set; }
    }
}
