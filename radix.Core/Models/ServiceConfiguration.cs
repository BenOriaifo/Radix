using System;
using System.Collections.Generic;

namespace Radix.Core.Models
{
    public partial class ServiceConfiguration
    {
        public long Id { get; set; }
        public long MessageTypeId { get; set; }
        public int? MaximumRecordsToFetch { get; set; }
        public int? WaitTime { get; set; }

        public MessageType MessageType { get; set; }
    }
}
