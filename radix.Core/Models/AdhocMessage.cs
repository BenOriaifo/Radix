using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class AdhocMessage
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Smsmessage { get; set; }
        public string EmailMessage { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? Status { get; set; }
        public string Recipient { get; set; }
    }
}
