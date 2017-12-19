using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThaiTable.Models
{
    public class Product
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public Product(string id)
        {
            this.Id = id;
        }
    }
}
