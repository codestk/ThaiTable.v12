namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ModifierOfOrder
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ProductID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string ModifierID { get; set; }

        public int Qty { get; set; }

        public virtual OrderItem OrderItem { get; set; }

        public ModifierOfOrder()
        {

        }

        public ModifierOfOrder(string productID, string modifierID, int qty)
        {
            //OrderID = orderID;
            ProductID = productID;
            ModifierID = modifierID;
            Qty = qty;
        }
    }
}
