namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Food
    {
        [StringLength(20)]
        public string FoodID { get; set; }

        public string FoodName { get; set; }

        public string FoodCode { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Thumbnail { get; set; }

        public string ImageName { get; set; }

        [StringLength(2)]
        public string Category_CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}
