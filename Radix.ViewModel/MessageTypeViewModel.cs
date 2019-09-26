using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radix.ViewModel
{
    public partial class MessageTypeViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
