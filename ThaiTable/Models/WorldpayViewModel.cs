using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class WorldpayViewModel
    {
        public string OrderID { get; set; }
        public string Description { get; set; }
        public string Total { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
    }
}