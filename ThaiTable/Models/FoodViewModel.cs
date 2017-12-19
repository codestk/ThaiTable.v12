using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class FoodViewModel
    {
        public string FoodId { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
        public List<ModifierViewModel> Modifiers { get; set; }

        public FoodViewModel()
        {
            Modifiers = new List<ModifierViewModel>();
        }
    }
}