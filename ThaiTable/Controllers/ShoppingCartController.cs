using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiTable.Models;

namespace ThaiTable.Controllers
{
    public class ShoppingCartController : Controller
    {
        FoodContext db = new FoodContext();
        ShoppingCart cart = SessionHelper.Read<ShoppingCart>(ShoppingCart.SHOPPING_CART_SESSION);
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View(cart);
        }

        public ActionResult ShoppingCartPartial()
        {
            return View("_ShoppingCart", cart);
        }

        public ActionResult AddItem(string foodId, int quantity, string modifierId)
        {
            var food = db.Foods.Where(c => c.FoodID == foodId).FirstOrDefault();
            Food modifier = null;
            if (modifierId != "")
            {
                modifier = (from f in db.Foods
                            from m in db.Modifiers.Where(c => c.FoodCode == f.FoodCode)
                            from fm in db.Foods.Where(c =>c.FoodCode==modifierId && c.FoodCode == m.ModifierId)
                            select fm).FirstOrDefault();
            }
            if (food != null)
            {
                cart.AddItem(foodId, food.FoodName, food.Price, quantity, food.ImageName, modifierId, 
                    modifier==null?null:modifier.FoodName, modifier==null?0:modifier.Price);

            }
            return new EmptyResult();
        }

        public ActionResult RemoveItem(string foodId, string res)
        {
            var food = db.Foods.Where(c => c.FoodID == foodId).FirstOrDefault();
            if (food != null)
            {
                cart.RemoveItem(foodId, res);

                // update db
                //Order order = GetOrder(cart.CartID);
                //order.RemoveItem(foodId);
                //db.SaveChanges();
            }
            return new EmptyResult();
        }

        public ActionResult UpdateDelivery(int shipping)
        {
            cart.IsShipping = shipping == 1 ? true : false;

            return View("Index", cart);
        }

        private void UpdateQuantity(string productId, int quantity, string res)
        {
            var food = db.Foods.Where(c => c.FoodID == productId).FirstOrDefault();
            if (food != null)
            {
                cart.SetItemQuantity(productId, quantity, res);

                // update db
                //Order order = GetOrder(cart.CartID);
                //order.UpdateItem(productId, quantity);
                ////db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
            }

            //return ShoppingCartPartial();
        }

        [HttpPost]
        public ActionResult UpdateQuantity(List<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                UpdateQuantity(item.ProductId, item.Quantity, item.ResId);
            }

            return PartialView("_ShoppingCart", cart);
        }

        //public ActionResult GetDeliveryRanges()
        //{
        //    const int minuteRange = 15;
        //    // 9:00 - 15:00
            

            
        //}



    }
}