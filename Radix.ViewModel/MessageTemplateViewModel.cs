using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radix.ViewModel
{
    public partial class MessageTemplateViewModel
    {
        public int Id { get; set; }

        [Required]
        public long MessageTypeId { get; set; }

        public string Smstemplate { get; set; }
        public string EmailTemplate { get; set; }

    }
}
