using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radix.ViewModel
{
    public partial class ServiceConfigurationViewModel
    {
        public long Id { get; set; }
        [Required]
        public long MessageTypeId { get; set; }
        [Required]
        public int MaximumRecordsToFetch { get; set; }
        [Required]
        public int WaitTime { get; set; }

        public MessageTypeViewModel MessageType { get; set; }
    }
}
