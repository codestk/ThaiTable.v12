using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class PaymentModel
    {
        public WorldpayViewModel Worldpay{ get; set; }
        public CardModel Card { get; set; }
        public CustomerModel Customer { get; set; }
    }
}