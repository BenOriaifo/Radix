using System;
using System.Collections.Generic;

namespace Radix.Data
{
    public partial class AdhocMessages
    {
        public long Id { get; set; }
        public string Smsmessage { get; set; }
        public string EmailMessage { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? Status { get; set; }
        public string Recipient { get; set; }
    }
}
