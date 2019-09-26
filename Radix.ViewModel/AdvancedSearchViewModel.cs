using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.ViewModel
{
    public class AdvancedSearchViewModel
    {
        public DateTime? DateExtractedFrom { get; set; }
        public DateTime? DateExtractedTo { get; set; }
        public DateTime? DateSentFrom { get; set; }
        public DateTime? DateSentTo { get; set; }
        public string EmployerName { get; set; }
        public string EmployerCode { get; set; }
    }
}
