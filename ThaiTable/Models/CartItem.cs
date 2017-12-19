
using System;

namespace ThaiTable.Models
{


    /**
     * The CartItem Class
     * 
     * Basically a structure for holding item data
     */
    public class CartItem : IEquatable<CartItem>
    {
        #region Properties

        // A place to store the quantity in the cart
        // This property has an implicit getter and setter.
        public int Quantity { get; set; }

        private string _productId;
        public string ProductId
        {
            get { return _productId; }
            set
            {
                // To ensure that the Prod object will be re-created
                _product = null;
                _productId = value;
            }
        }

        private Product _product = null;
        public Product Product
        {
            get
            {
                // Lazy initialization - the object won't be created until it is needed
                if (_product == null)
                {
                    _product = new Product(ProductId);
                }
                return _product;
            }
        }

        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public string ImagePath { get; set; }

        public string ResId { get; set; }
        public string ResName { get; set; }
        public double ResPrice { get; set; }

        public double TotalPrice
        {
            get { return UnitPrice * Quantity + ResPrice * Quantity; }
        }

        #endregion

        public CartItem(){ }

        // CartItem constructor just needs a productId
        public CartItem(string productId)
        {
            this.ProductId = productId;
        }

        public CartItem(string productId, string description, double price, int quantity, string imagePath)
        {
            this.ProductId = productId;
            this.Description = description;
            this.UnitPrice = price;
            this.Quantity = quantity;
            this.ImagePath = imagePath;

        }

        /**
         * Equals() - Needed to implement the IEquatable interface
         *    Tests whether or not this item is equal to the parameter
         *    This method is called by the Contains() method in the List class
         *    We used this Contains() method in the ShoppingCart AddItem() method
         */
        public bool Equals(CartItem item)
        {
            return item.ProductId == this.ProductId;
        }
    }
}
