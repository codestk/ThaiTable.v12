using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiTable.Models;

namespace ThaiTable.Controllers
{
    public class HomeController : Controller
    {
        private FoodContext db = new FoodContext();
        ShoppingCart cart = SessionHelper.Read<ShoppingCart>(ShoppingCart.SHOPPING_CART_SESSION);

        public ActionResult Index()
        {
            Session.Remove(Constants.SESSION_ORDER_TYPE);
            Session.Remove(Constants.SESSION_CATEGORY);
            Session.Remove(ShoppingCart.SHOPPING_CART_SESSION);
            Session.Remove(Constants.SESSION_DELIVERY_TIME);
            SessionHelper.Write(ShoppingCart.SHOPPING_CART_SESSION, new ShoppingCart());
            cart.ClearCart();
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Welcome(string orderType)
        {
            Session[Constants.SESSION_ORDER_TYPE] = orderType;

            var category = db.Categories.FirstOrDefault();
            if (category != null)
            {
                Session[Constants.SESSION_CATEGORY] = category.CategoryID;
                ViewBag.CategoryId = category.CategoryID;
            }

            cart.IsShipping = orderType.Equals("02") ? true : false;

            return RedirectToAction("Menu");
        }

        public ActionResult Menu(string categoryId = null)
        {                      
            ViewBag.State = 1;

            categoryId = categoryId ?? Session[Constants.SESSION_CATEGORY].ToString();
            ViewBag.CategoryId = categoryId;
            return View();//Menu(Session["orderType"].ToString(), Session["categoryId"].ToString());
        }

        public ActionResult SelectedMenu(string categoryId = null)
        {
            if (categoryId != null)
            {
                Session[Constants.SESSION_CATEGORY] = categoryId;
            }
            return RedirectToAction("Menu");
        }

        //public ActionResult Menu(string orderType, string categoryId = null)
        //{
        //    ViewBag.State = 1;
        //    Session["orderType"] = orderType;            
        //    Session["categoryId"] = categoryId;
        //    ViewBag.CategoryId = categoryId;

        //    cart.IsShipping = orderType.Equals("02") ? true : false;

        //    return View();
        //}

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult CategoryLayout(string categoryId = null)
        {
            ViewBag.CategoryId = categoryId;
            if (db.Categories.Count() > 0)
            {
                var categories = db.Categories.OrderBy(c=>c.CategoryID).ToList();
                if (categoryId == null)
                {
                    if (Session[Constants.SESSION_CATEGORY] == null)
                    {
                        ViewBag.CategoryId = categories.FirstOrDefault().CategoryID;
                        Session[Constants.SESSION_CATEGORY] = categoryId;
                    }
                    else
                    {
                        ViewBag.CategoryId = Session[Constants.SESSION_CATEGORY];
                    }
                }

                return PartialView("_CategoryLayout", categories);
            }
            else
            {
                return PartialView("_CategoryLayout", null);
            }
        }

        public ActionResult FoodList(string categoryId = null)
        {
            //Category category = null;
            if (categoryId == null || categoryId == "undefined")
            {
                categoryId = db.Categories.FirstOrDefault().CategoryID;
            }

            //ViewBag.CategoryId = categoryId;
            //else
            //{
            //    category = db.Categories.Where(c => c.CategoryID == categoryId.ToString()).SingleOrDefault();
            //}
            //ViewBag.Category = category;

            var foods = (from c in db.Foods.Include("Category")
                         where c.Category.CategoryID == categoryId
                         select new FoodViewModel
                         {
                             FoodId = c.FoodID,
                             FoodName = c.FoodName,
                             Description = c.Description,
                             Price = c.Price,
                             Category = c.Category
                         }).OrderBy(c=>c.FoodName).ToList();
            var modifiers = (from food in db.Foods
                             from modifier in db.Modifiers.Where(c => c.FoodCode == food.FoodCode)
                             from fm in db.Foods.Where(c => c.FoodCode == modifier.ModifierId)
                             select new ModifierViewModel
                             {
                                 FoodId = food.FoodID,                                   
                                 ModifierId = modifier.ModifierId,
                                 ModifierName = fm.FoodName,
                                 Price = fm.Price
                             }).ToList();

            foreach (var food in foods)
            {
                var modifier = (from c in modifiers
                               where c.FoodId == food.FoodId
                               select c).ToList();

                food.Modifiers.AddRange(modifier.ToList());
            }

            //foods[0].Modifiers.Add(new ModifierViewModel { FoodId="0020", ModifierId="40", ModifierName="Small", Price=2});
            //foods[0].Modifiers.Add(new ModifierViewModel { FoodId = "0020", ModifierId = "41", ModifierName = "Large", Price = 5 });
    

            return View(foods);
        }

    }
}