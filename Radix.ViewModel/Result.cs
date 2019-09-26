using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.ViewModel
{
    public class Result
    {
        public string messageId { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public string text { get; set; }
        public DateTime sentAt { get; set; }
        public DateTime doneAt { get; set; }
        public int smsCount { get; set; }
        public string mccMnc { get; set; }
        public Price price { get; set; }
        public Status status { get; set; }
        public Error error { get; set; }
    }
}
