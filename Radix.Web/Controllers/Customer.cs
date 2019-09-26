using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Web.Controllers
{
    public class Customer
    {
        public long Id { get; set; }
        public string Pin { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
