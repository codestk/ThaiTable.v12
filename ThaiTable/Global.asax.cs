using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThaiTable.Models;

namespace ThaiTable
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (SessionHelper.Read<ShoppingCart>(ShoppingCart.SHOPPING_CART_SESSION) == null)
                SessionHelper.Write(ShoppingCart.SHOPPING_CART_SESSION, new ShoppingCart());
            // event is raised each time a new session is created     
        }

        protected void Session_End(object sender, EventArgs e)
        {
            ShoppingCart cart = SessionHelper.Read<ShoppingCart>(ShoppingCart.SHOPPING_CART_SESSION);
            if (cart != null)
            {
                Session.Remove(ShoppingCart.SHOPPING_CART_SESSION);
                cart = null;
            }
            // event is raised when a session is abandoned or expires
        }
    }
}
