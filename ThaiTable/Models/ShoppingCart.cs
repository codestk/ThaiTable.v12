using System.Collections.Generic;
using System.Web;
using System.Linq;
using System;

namespace ThaiTable.Models
{
    public enum CartStatus
    {
        Unsuccess = 0,
        Success
    }

    /**
     * The ShoppingCart class
     * 
     * Holds the items that are in the cart and provides methods for their manipulation
     */
    public class ShoppingCart
    {
        #region Properties

        public const string SHOPPING_CART_SESSION = "ShoppingCart";
        public Guid CartID { get; set; }
        //public DateTime CartDate { get; set; }
        //public CartStatus Status { get; set; }
        public bool IsShipping { get; set; }
        public List<CartItem> Items { get; private set; }

        #endregion

        #region Singleton Implementation

        // Readonly properties can only be set in initialization or in a constructor
        //public static readonly ShoppingCart Instance;
        private double FREE_SHIPPING_LIMIT = 50;
        private double SHIPPING_FEE = 2.95;

        // The static constructor is called as soon as the class is loaded into memory
        public ShoppingCart()
        {
            // If the cart is not in the session, create one and put it there
            // Otherwise, get it from the session
            //if (HttpContext.Current.Session["ASPNETShoppingCart"] == null)
            //{
                //Instance = new ShoppingCart();
                CartID = Guid.NewGuid();
                IsShipping = true;
                Items = new List<CartItem>();
                //HttpContext.Current.Session["ShoppingCart"] = Instance;
            //}
            //else
            //{
              //  Instance = (ShoppingCart)HttpContext.Current.Session["ASPNETShoppingCart"];
            //}
        }

        // A protected constructor ensures that an object can't be created from outside
        //protected ShoppingCart() { }        

        #endregion

        #region Item Modification Methods
        /**
     * AddItem() - Adds an item to the shopping 
     */
        public void AddItem(string productId, string description, double price, int quantity, string imagePath, string modifierId, string modifierName, double modPrice)
        {
            // Create a new item to add to the cart
            CartItem newItem = new CartItem(productId, description, price, quantity, imagePath);

            // If this item already exists in our list of items, increase the quantity
            // Otherwise, add the new item to the list

            //string res = modifierId == "" ? null : modifierId;

            var existingItem = (from c in Items
                               where c.ProductId==productId && c.ResId == modifierId
                               select c).FirstOrDefault();
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.ResId = modifierId;
                existingItem.ResName = modifierName;
                existingItem.ResPrice = modPrice;
            }
            else
            {
                newItem.Quantity = quantity;
                newItem.ResId = modifierId;
                newItem.ResName = modifierName;
                newItem.ResPrice = modPrice;

                Items.Add(newItem);
            }
        }

        /**
         * SetItemQuantity() - Changes the quantity of an item in the cart
         */
        public void SetItemQuantity(string productId, int quantity, string res)
        {
            // If we are setting the quantity to 0, remove the item entirely
            if (quantity == 0)
            {
                RemoveItem(productId, res);
                return;
            }

            // Find the item and update the quantity
            CartItem updatedItem = (from c in Items
                               where c.ProductId==productId && c.ResId == res
                               select c).FirstOrDefault();

            if (updatedItem != null)
                updatedItem.Quantity = quantity;
        }

        /**
         * RemoveItem() - Removes an item from the shopping cart
         */
        public void RemoveItem(string productId, string res)
        {
            //CartItem removedItem = (from c in Items
            //                        where c.ProductId == productId && c.ResId.Equals(res)
            //                        select c).FirstOrDefault();
            //if (removedItem != null)
                Items.RemoveAll(c=>c.ProductId==productId && c.ResId==res);
        }
        #endregion

        public void ClearCart()
        {
            Items.Clear();
        }

        #region Reporting Methods
        /**
     * GetSubTotal() - returns the total price of all of the items
     *                 before tax, shipping, etc.
     */
        public double GetTotal()
        {
            double total = 0;
            foreach (CartItem item in Items)
                total += item.TotalPrice;

            return total;
        }

        public double GetGrandTotal()
        {
            return GetTotal() + GetShippingFee();
        }

        public double GetShippingFee()
        {
            double fee = 0;
            if ((Items.Count()>0 && GetTotal() < FREE_SHIPPING_LIMIT) && IsShipping)
                fee = SHIPPING_FEE;
            return fee;
        }

        /*
         * Number of item in cart
         */
        public int GetItemCount()
        {
            return Items.Sum(c => c.Quantity);
        }
        #endregion
    }
}