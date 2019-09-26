using System;
using System.Collections.Generic;

namespace Radix.Data
{
    public partial class ServiceConfigurations
    {
        public long Id { get; set; }
        public int? MessageTypeId { get; set; }
        public int? MaximumRecordsToFetch { get; set; }
        public int? WaitTime { get; set; }

        public MessageTypes MessageType { get; set; }
    }
}
