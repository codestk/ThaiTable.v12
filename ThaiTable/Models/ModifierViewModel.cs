using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class ModifierViewModel
    {
        public string FoodId { get; set; }
        public string ModifierId { get; set; }
        public string ModifierName { get; set; }
        public double Price { get; set; }
    }
}