using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class CustomerModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }

        public string Country { get; set; }
    }
}