namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string productID { get; set; }

        public int Amount { get; set; }

        [StringLength(1)]
        public string MoveToClient { get; set; }

        public virtual Order Order { get; set; }

        public virtual ICollection<ModifierOfOrder> ModifierOfOrders { get; set; }

        public OrderItem() {
            ModifierOfOrders = new HashSet<ModifierOfOrder>();
        }
        public OrderItem(string productId, int amount, string modifierId): this()
        {
            this.productID = productId;
            this.Amount = amount;

            if (modifierId != "")
            {
                ModifierOfOrders.Add(new ModifierOfOrder(productID, modifierId, amount));
            }
            //this.ModifierId = modifierId;
        }
    }
}
