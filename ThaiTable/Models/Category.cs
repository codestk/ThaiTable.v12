namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        public Category()
        {
            Foods = new HashSet<Food>();
        }

        [StringLength(2)]
        public string CategoryID { get; set; }

        [StringLength(100)]
        public string CategoryName { get; set; }

        public virtual ICollection<Food> Foods { get; set; }
    }
}
