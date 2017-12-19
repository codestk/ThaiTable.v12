namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Order
    {

        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public bool IsPaid { get; set; }

        public Guid RowID { get; set; }
        public string TxnId { get; set; }

        public double FeeShipping { get; set; }

        public bool Shipping { get; set; }
        public DateTime DeliveryDT { get; set; }

        public virtual Customer Customer { get; set; }
        [StringLength(1)]
        public string MoveToClient { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderDate = DateTime.Now;
            OrderItems = new HashSet<OrderItem>();
        }
        public Order(Guid id)
            : this()
        {
            this.RowID = id;
        }

        public void AddItem(string productID, int amount, string modifierId)
        {
            OrderItem item = (from c in OrderItems
                              where c.productID == productID
                              select c).SingleOrDefault();
            ModifierOfOrder modItem = null;

            if (item == null)
                OrderItems.Add(new OrderItem(productID, amount, modifierId));
            else
            {
                item.Amount += amount;

                if (modifierId != "")
                {
                    modItem = (from c in item.ModifierOfOrders
                               where c.ModifierID == modifierId
                               select c).FirstOrDefault();

                    if (modItem == null)
                    {
                        item.ModifierOfOrders.Add(new ModifierOfOrder(item.productID, modifierId, amount));
                    }
                }

            }
        }


        internal void RemoveItem(string productID)
        {
            OrderItem item = (from c in OrderItems
                              where c.productID == productID
                              select c).SingleOrDefault();
            if (item == null)
                throw new InvalidOperationException("Unknown item in shopping cart.");
            else
                OrderItems.Remove(item);
        }

        public void UpdateItem(string productId, int quantity)
        {
            OrderItem item = (from c in OrderItems
                              where c.productID == productId
                              select c).SingleOrDefault();
            if (item != null)
                item.Amount = quantity;
        }
    }
}
