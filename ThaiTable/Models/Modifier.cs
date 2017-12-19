namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Modifier
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string FoodId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ModifierId { get; set; }
        public string FoodCode { get; set; }
    }
}
