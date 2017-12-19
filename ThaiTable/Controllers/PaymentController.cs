using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ThaiTable.Models;
using WorldPayDotNet.Common;
using WorldPayDotNet.Hosted;

namespace ThaiTable.Controllers
{
    public class PaymentController : Controller
    {
        FoodContext db = new FoodContext();
        ShoppingCart cart = SessionHelper.Read<ShoppingCart>(ShoppingCart.SHOPPING_CART_SESSION);

        // GET: Payment
        public ActionResult Index()
        {
            ViewBag.CategoryId = Session[Constants.SESSION_CATEGORY];
            if (cart.Items.Count() == 0)
                return RedirectToAction("Index", "Home");
            //return RedirectToAction("Menu", "Home", new { orderType = Session["orderType"], categoryId = ViewBag.CategoryId });

            ViewBag.State = 2;
            //CollectionViewModel model = null;
            if (Session[Constants.SESSION_ORDER_TYPE].ToString() == "01")
                ViewBag.Collection = new CollectionViewModel();
            else
                ViewBag.Delivery = new DeliveryViewModel();

            //if (orderType == "02")
            //{
            //}


            return View();
        }

        [HttpPost]
        public ActionResult Collection(CollectionViewModel customer)
        {
            return Customer(customer);
        }

        [HttpPost]
        public ActionResult Delivery(DeliveryViewModel customer)
        {

            return Customer(customer);

            //return View("Index");
        }

        private ActionResult Customer(object customer)
        {
            var errors = ModelState
.Where(x => x.Value.Errors.Count > 0)
.Select(x => new { x.Key, x.Value.Errors })
.ToArray();

            //if (Session["orderType"].ToString() == "" || cart.Items.Count() == 0)
            //    return RedirectToAction("Index", "Home");
            string dt = "";
            if (Session[Constants.SESSION_DELIVERY_TIME] != null)
                dt = Session[Constants.SESSION_DELIVERY_TIME].ToString();
            if (ModelState.IsValid && !dt.Equals(""))
            {
                if (cart.Items.Count() == 0)
                    return RedirectToAction("Menu", "Home");

                // update db
                Order order = GetOrder(cart.CartID);
                if (Session[Constants.SESSION_DELIVERY_TIME] != null)
                    order.DeliveryDT = ConvertToDateTime(Session[Constants.SESSION_DELIVERY_TIME].ToString());
                foreach (var item in cart.Items)
                {
                    order.AddItem(item.ProductId, item.Quantity, item.ResId == "undefined" ? null : item.ResId);
                }

                order.FeeShipping = cart.GetShippingFee();

                Customer newCustomer = null;
                if (customer is CollectionViewModel)
                {
                    CollectionViewModel c = (CollectionViewModel)customer;

                    newCustomer = new Customer();
                    newCustomer.FirstName = c.FirstName;
                    newCustomer.LastName = c.LastName;
                    newCustomer.PhoneNo = c.ContactNo;
                }
                else if (customer is DeliveryViewModel)
                {
                    var deliveryCustomer = (DeliveryViewModel)customer;

                    newCustomer = new Customer();
                    newCustomer.FirstName = deliveryCustomer.FirstName;
                    newCustomer.LastName = deliveryCustomer.LastName;
                    newCustomer.PhoneNo = deliveryCustomer.ContactNo;
                    newCustomer.Address1 = deliveryCustomer.Address.Replace(",", "");
                    newCustomer.Address1 = newCustomer.Address1.Replace("'", "");
                    newCustomer.Postcode = deliveryCustomer.Postcode;
                    newCustomer.Info = deliveryCustomer.Email;
                }

                order.Customer = newCustomer;
                //db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.Orders.Add(order);
                db.SaveChanges();

                StringBuilder sb = new StringBuilder();
                //string host = Request.Url.Host;
                //string appPath = Request.ApplicationPath;
                //string temp = host + appPath + "Paypal/Notify";

                string description = "";
                foreach (var item in cart.Items)
                {
                    description += item.Quantity + "x" + item.Description;
                    if (item.ResId != "")
                        description += " " + item.ResName;
                    description += "\n";
                }

                WorldpayViewModel model = new WorldpayViewModel();
                model.OrderID = "WEB" + string.Format("{0:00}", order.OrderID);
                model.Description = description;
                model.Total = cart.GetGrandTotal().ToString();
                model.CustomerName = newCustomer.FullName;
                model.Email = newCustomer.Info;

                if (customer is DeliveryViewModel)
                {
                    model.Address = newCustomer.Address1;
                    model.PostCode = newCustomer.Postcode;
                }
                ViewBag.WorldpayModel = model;


                //sb.Append("<form id='paypalForm' action='https://select.worldpay.com/wcc/purchase' method='post'>");
                //sb.Append("<input type='hidden' name='instId' value='1018224' />");
                //sb.Append("<input type='hidden' name='cartId' value='WEB" + string.Format("{0:00}", order.OrderID) + "' />");
                //sb.Append("<input type='hidden' name='amount' value='" + cart.GetGrandTotal() + "' />");
                //sb.Append("<input type='hidden' name='currency' value='GBP' />");
                //sb.Append("<input type='hidden' name='desc' value='" + description + "' />");
                //sb.Append("<input type='hidden' name='testMode' value='100' />");
                //sb.Append("<input type='hidden' name='MC_callback' value='http://Ns3.sbcserver.com/WorldPay/WorldPayCallback'>");
                //sb.Append("<input type='hidden' name='name' value='" + customer.FullName + "' />");
                //sb.Append("<input type='hidden' name='address' value='" + customer.Address1 + "' />");
                //sb.Append("<input type='hidden' name='postcode' value='+" + customer.Postcode + "' />");
                ////sb.Append("<input type='hidden' name='country' value='"+customer.Postcode+"' />");
                ////sb.Append("<input type='hidden' name='email' value='"+customer.Email+"' />");
                //sb.Append("</form>");

                //cart.ClearCart();
                //Session.Remove(ShoppingCart.SHOPPING_CART_SESSION);
                //Session.Remove("DeliveryTime");
                //SessionHelper.Write(ShoppingCart.SHOPPING_CART_SESSION, new ShoppingCart());
                //Session["orderType"] = "";

                //return Content(sb.ToString());

                //if (customer is DeliveryViewModel)
                //WorldpayRedirect redirect = Worldpay.ExpressCheckout(model);
                //return new RedirectResult(redirect.Url);

                return Pay(model);
                //return View("Index");
                //else
                //    return RedirectToAction("Index", "Home");
            }
            ViewBag.State = 2;
            ViewBag.WorldpayModel = null;
           
            //string viewName="";
            //if (Session["orderType"].ToString() == "01")
            //    viewName = "_ForCollection";
            //else if (Session["orderType"].ToString() == "02")
            //    viewName = "_ForDelivery";
            return View("Index");

        }

        public ActionResult Pay(WorldpayViewModel model)
        {
            ViewBag.WorldpayModel = model;

            //Retrieve the InstallationID, MD5SecretKey and SiteBaseURL values from the web.config
            int installationID = Convert.ToInt32(WebConfigurationManager.AppSettings["InstallationID"]);
            string MD5secretKey = WebConfigurationManager.AppSettings["MD5secretKey"];
            string WebsiteURL = WebConfigurationManager.AppSettings["WebsiteURL"];
            int testMode = Convert.ToInt32(WebConfigurationManager.AppSettings["testMode"]);

            HostedTransactionRequest PRequest = new HostedTransactionRequest();
            PRequest.instId = installationID;
            //amount - A decimal number giving the cost of the purchase in terms of the major currency unit e.g. 12.56
            PRequest.amount = Convert.ToDouble(model.Total);
            //cartId - If your system has created a unique order/cart ID, enter it here.
            PRequest.cartId = string.Format("{0:00}", model.OrderID);//Guid.NewGuid().ToString();
            //desc - enter the description for this order.
            PRequest.desc = model.Description;
            //currency - 3 letter ISO code for the currency of this payment.
            PRequest.currency = "GBP";
            //name, address1/2/3, town, region, postcode & country - Billing address fields
            PRequest.name = model.CustomerName;
            PRequest.address1 = model.Address;
            //PRequest.address2 = TxtAddress2.Text;
            //PRequest.address3 = TxtAddress3.Text;
            //PRequest.town = "BKK";
            //PRequest.region = TxtRegion.Text;
            PRequest.postcode = model.PostCode;
            //PRequest.country = DdlCountry.SelectedValue;
            ////tel - Shopper's telephone number
            //PRequest.tel = TxtTelephone.Text;
            ////fax - Shopper's fax number
            //PRequest.fax = TxtFax.Text;
            ////email - Shopper's email address
            PRequest.email = model.Email;
            ////If passing delivery details, set withDelivery = true.
            //PRequest.withDelivery = true;
            ////delvName, delvAddress1/2/3, delvTown, delvRegion, delvPostcode & delvCountry - Delivery address fields (NOTE: do not need to be passed/set if "withDelivery" set to false.
            PRequest.delvName = model.CustomerName;
            PRequest.delvAddress1 = model.Address;
            //PRequest.delvAddress2 = TxtdelvAddress2.Text;
            //PRequest.delvAddress3 = TxtdelvAddress3.Text;
            //PRequest.delvTown = "ABC";
            //PRequest.delvRegion = TxtdelvRegion.Text;
            PRequest.delvPostcode = model.PostCode;
            //PRequest.delvCountry = DdldelvCountry.SelectedValue;
            //authMode - set to TransactionType.A for Authorise & Capture, set to TransactionType.E for Pre-Auth Only.
            PRequest.authMode = TransactionType.A;
            //testMode - set to 0 for Live Mode, set to 100 for Test Mode.
            PRequest.testMode = testMode;
            //hideCurrency - Set to true to hide currency drop down on the hosted payment page.
            PRequest.hideCurrency = false;
            //fixContact - Set to true to stop a shopper from changing their billing/shipping addresses on the hosted payment page.
            PRequest.fixContact = false;
            //hideContact - set to true to hide the billing/shipping address fields on the hosted payment page.
            PRequest.hideContact = false;
            //MC_callback - the URL of the Callback.aspx file. SiteBaseURL is set in the web.config file.
            PRequest.MC_callback = WebsiteURL + "/Payment/WorldPayCallback";

            HttpContext httpa = default(HttpContext);
            httpa = System.Web.HttpContext.Current;
            HostedPaymentProcessor process = new HostedPaymentProcessor(httpa);
            process.SubmitTransaction(PRequest, MD5secretKey);

            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult WorldPayCallback()
        {
            try
            {
                //Get callbackPW and MD5secretkey from web.config
                string CallbackPW = WebConfigurationManager.AppSettings["CallbackPW"];
                string MD5secretKey = WebConfigurationManager.AppSettings["MD5secretKey"];

                HttpContext context = System.Web.HttpContext.Current;
                //CallbackResult - Validates Callback - populated with the post variables sent back to the callback URL.
                CallbackResult CallbackResult = new CallbackResult(context.Request.Form, MD5secretKey, CallbackPW);

                //NOTE: CallbackResult.<parameter> can be used to return individual callback parameters
                // - e.g. CallbackResult.transId, CallbackResult.amount etc.
                // - see http://www.worldpay.com/support/kb/bg/paymentresponse/pr5201.html for a full list of callback parameters.

                //Check returned transaction status
                switch (CallbackResult.transStatus)
                {
                    //Y = Successful
                    case "Y":
                        //Transaction Successful - Add your code to process a successful transaction here (before the break).

                        int id = int.Parse(CallbackResult.cartId.Substring(3));
                        Order order = (from c in db.Orders
                                       where c.OrderID == id
                                       select c).FirstOrDefault();
                        if (order == null)
                        {
                            return View("Not found.");
                        }
                        else
                        {
                            string txn_id = Request.QueryString["txn_id"]; //Keep this ID to avoid processing the transaction twice
                            // update order
                            order.TxnId = txn_id;
                            order.IsPaid = true;
                            db.SaveChanges();
                            // success we delete shopping cart

                            //return RedirectToAction("Index", "Home");

                        }
                        Response.Write("Transaction Successful - Transaction ID: " + CallbackResult.transId);
                        break;

                    //C = Cancelled
                    case "C":
                        //Transaction Cancelled - Add your code to process a cancelled transaction here (before the break).

                        Response.Write("Transaction Cancelled.");
                        //return View("Transaction Cancelled.");

                        break;
                }
            }
            catch (Exception ex)
            {
                //Respond with StatusCode 500 - gateway will pick this up and send out an error email with further details.
                Response.StatusCode = 500;
                Response.Write(ex.Message);
            }

            return RedirectToAction("Index", "Home");

            //string wp_cartid = Request.Form["cartid"];


            // notify from WorldPay
            //string wp_rawauthcode = Request.Form["rawauthcode"];
            //string wp_amount = Request.Form["amount"];
            //string wp_installation = Request.Form["installation"];
            //string wp_tel = Request.Form["tel"];
            //string wp_address = Request.Form["address"];
            //string wp_mc_log = Request.Form["mc_log"];
            //string wp_rawauthmessage = Request.Form["rawauthmessage"];
            //string wp_authamount = Request.Form["authamount"];
            //string wp_amountstring = Request.Form["amountstring"];
            //string wp_cardtype = Request.Form["cardtype"];
            //string wp_avs = Request.Form["avs"];
            //string wp_cost = Request.Form["cost"];
            //string wp_currency = Request.Form["currency"];
            //string wp_testmode = Request.Form["testmode"];
            //string wp_authamountstring = Request.Form["authamountstring"];
            //string wp_fax = Request.Form["fax"];
            //string wp_transstatus = Request.Form["transstatus"];
            //string wp_compname = Request.Form["compname"];
            //string wp_postcode = Request.Form["postcode"];
            //string wp_authcost = Request.Form["authcost"];
            //string wp_desc = Request.Form["desc"];
            ////string wp_cartid = Request.Form["cartid"];
            //string wp_transid = Request.Form["transid"];
            //string wp_callbackpw = Request.Form["callbackpw"];
            //string wp_sessionId = Request.Form["MC_sessionId"];
            //string wp_CusId = Request.Form["MC_cusId"];
            //string wp_authmode = Request.Form["authmode"];
            //string wp_name = Request.Form["name"];
            //string wp_shop = Request.Form["MC_shop"];
            //string wp_wafMerchMessage = Request.Form["wafMerchMessage"];
            //string wp_authentication = Request.Form["authentication"];
            //string wp_email = Request.Form["email"];


            ////string receiver_email = Request.QueryString["receiver_email"]; //Check email address to make sure that this is not a spoof

            //if (wp_transstatus == "Y")
            //{
                //}
            //}

            //return View("Error");

        }

        [HttpPost]
        public ActionResult WorldPayNotify()
        {
            return new EmptyResult();
        }

        private Order GetOrder(Guid id)
        {
            Order order = (from c in db.Orders.Include("OrderItems")
                           where c.RowID == id
                           select c).SingleOrDefault();
            if (order == null)
            {
                order = new Order(id);
                //db.Orders.Add(order);
                //db.SaveChanges();
            }

            return order;
        }

        public ActionResult DeliveryTimePartial()
        {
            ViewData["TimeTitle"] = Session["orderType"].ToString().Equals("01") ? "COLLECTION TIME" : "DELIVERY TIME";
            ViewData["options"] = GetTimesList().ToArray();
            return View("_DeliveryTime");
        }

        [HttpPost]
        public ActionResult DeliveryTime(string time)
        {
            Session[Constants.SESSION_DELIVERY_TIME] = time;

            return new EmptyResult();
        }

        private DateTime ConvertToDateTime(string time)
        {
            DateTime deliveryTime = DateTime.Now.Date;

            string[] token = time.Split(' ');
            int hour = int.Parse(token[1].Substring(0, 2));
            int minute = int.Parse(token[1].Substring(3, 2));

            if (token[0].Equals("TOMORROW"))
            {
                // tomorrow
                deliveryTime = deliveryTime.AddDays(1);
            }
            deliveryTime = deliveryTime.AddHours(hour);
            deliveryTime = deliveryTime.AddMinutes(minute);

            return deliveryTime;

        }

        private List<SelectListItem> GetTimesList()
        {
            List<String> timeSlots = null;

            string type = Session["orderType"].ToString();
            OrderType orderType = type.Equals("01") ? OrderType.Collection : OrderType.Delivery;
            AssignShopTimes(orderType);

            TimeSpan curTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            //var shop = GetShopTimeSlot(curTime);
            if (curTime >= closeTime)
            {
                // not in slot
                timeSlots = GetTimeTomorrow(orderType);
            }
            else
            {
                timeSlots = GetTimeToday(orderType, curTime);
            }


            //List<String> timeSlots = GetTimeToday(type.Equals("01") ? OrderType.Collection : OrderType.Delivery);


            //merge.AddRange(GetTimeTomorrow());

            var result = new List<SelectListItem>();
            var selectedItem = Session["DeliveryTime"];
            for (int i = 0; i < timeSlots.Count(); i++)
            {
                var item = new SelectListItem() { Value = i.ToString(), Text = timeSlots[i] };
                //if (i == 5)
                //    item.Selected = true;
                if (selectedItem != null)
                    if (timeSlots[i].Equals(selectedItem.ToString()))
                        item.Selected = true;
                result.Add(item);
            }

            return result;
        }

        //int slot = 0;
        TimeSpan openTime;// = TimeSpan.Parse("17:30");
        TimeSpan closeTime;// = TimeSpan.Parse("22:30");

        enum OrderType
        {
            Collection, Delivery
        }

        private void AssignShopTimes(OrderType orderType)
        {
            var shopTimes = db.ShopTimeSlots
                .Where(c=>c.OrderType == orderType.ToString().Substring(0, 1))
                .OrderBy(c => c.ID)
                .ToList();            
            openTime = shopTimes.FirstOrDefault().StartTime;
            closeTime = shopTimes.LastOrDefault().EndTime;

        }

        int GetSlotRange(OrderType orderType)
        {
            if (orderType == OrderType.Collection)
                return Convert.ToInt32(WebConfigurationManager.AppSettings["slot_collection"].ToString());
            else
                return Convert.ToInt32(WebConfigurationManager.AppSettings["slot_delivery"].ToString());

        }

        private List<TimeSpan> GetTimeSlots(OrderType orderType)
        {
            AssignShopTimes(orderType);
            int slot = GetSlotRange(orderType);
            // OPEN 7:30 PM - 10:30 PM
            var slots = new List<TimeSpan>();
            //TimeSpan curTime = openTime.Add(TimeSpan.FromMinutes(slot));
            TimeSpan curTime = openTime;
            while (curTime < closeTime.Add(TimeSpan.FromMinutes(slot)))
            {
                slots.Add(curTime);
                if (orderType == OrderType.Delivery)
                {
                    curTime = curTime.Add(TimeSpan.FromMinutes(15));
                    if (curTime == closeTime)
                        break;
                }
                else
                    curTime = curTime.Add(TimeSpan.FromMinutes(slot));
            }
            return slots;
        }

        private ShopTimeSlot GetShopTimeSlot(TimeSpan curTime)
        {
            var shopSlots = db.ShopTimeSlots.OrderBy(c => c.ID).ToList();
            ShopTimeSlot curSlot = null;
            foreach (var item in shopSlots)
            {
                if (item.StartTime <= curTime && item.EndTime >= curTime)
                {
                    curSlot = item;
                    break;
                }
            }
            if (curSlot == null)
            {
                if (curTime < shopSlots.FirstOrDefault().StartTime)
                    curSlot = shopSlots.FirstOrDefault();
                else
                    curSlot = shopSlots.LastOrDefault();
            }            

            return curSlot;

        }

        private int GetWeekSlot(OrderType orderType, TimeSpan curTime)
        {
            int day = (int)DateTime.Now.DayOfWeek;
            string type = orderType == OrderType.Collection ? "C" : "D";
            var shopSlot = GetShopTimeSlot(curTime);

            var weekSlot = db.WeekTimeSlots.Where(c => c.OrderType == type && c.Day == day && c.Slot == shopSlot.ID).FirstOrDefault();

            return weekSlot == null ? 0 : weekSlot.Minute;
        }

        private List<String> GetTimeToday(OrderType orderType, TimeSpan curTime)
        {
            //today
            //TimeSpan now = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            //TimeSpan gap = TimeSpan.FromMinutes(slot);
            //TimeSpan end = new TimeSpan(22, 30+slot, 0); 

            int slotMinute = GetWeekSlot(orderType, curTime);
            curTime = curTime.Add(TimeSpan.FromMinutes(slotMinute));

            var allSlots = GetTimeSlots(orderType);

            int i = 0;
            for (i = 0; i < allSlots.Count(); i++)
            {
                if (curTime < allSlots[i])
                    break;
            }

            int size = i==0 ? allSlots.Count() : allSlots.Count() - i;

            //TimeSpan[] slots =  new TimeSpan[size];
            //allSlots.CopyTo(slots, i);

            List<TimeSpan> slots = new List<TimeSpan>();
            for (int j = i; j < allSlots.Count(); j++)
            {
                slots.Add(allSlots[j]);
            }

            int slot = GetSlotRange(orderType);
            List<string> todayList = new List<string>();
            foreach (var item in slots)
            {
                //todayList.Add("TODAY " + item.ToString(@"hh\:mm"));
                if (orderType == OrderType.Collection)
                    todayList.Add("TODAY " + item.ToString(@"hh\:mm"));
                else
                {                    
                    todayList.Add("TODAY " + item.ToString(@"hh\:mm") + " - " + item.Add(TimeSpan.FromMinutes(slot)).ToString(@"hh\:mm"));
                }

            }
            if (orderType == OrderType.Delivery)
                todayList.RemoveAt(todayList.Count-1);
            //slots.CopyTo

            //if (start.Subtract(now).TotalMinutes < slot)
            //{
            //    start = start.Add(gap);
            //}

            //List<string> todayList = new List<string>();
            //TimeSpan ts = startTime;
            //while (ts <= closeTime)
            //{
            //    todayList.Add("TODAY " + ts.ToString(@"hh\:mm"));
            //    ts = ts.Add(gap);
            //}

            return todayList;
        }

        private List<String> GetTimeTomorrow(OrderType orderType)
        {
            //tomorrow
            //TimeSpan now = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            //TimeSpan gap = TimeSpan.FromMinutes(30);
            //TimeSpan end = new TimeSpan(24, 0, 0);
            //TimeSpan start = new TimeSpan(0, 0, 0);
            //if (start.Subtract(now).TotalMinutes < 30)
            //{
            //    start = start.Add(gap);
            //}

            int slot = GetSlotRange(orderType);

            List<string> tomorrowList = new List<string>();
            foreach (var item in GetTimeSlots(orderType))
            {
                if (orderType == OrderType.Collection)
                    tomorrowList.Add("TOMORROW " + item.ToString(@"hh\:mm"));
                else
                    tomorrowList.Add("TOMORROW " + item.ToString(@"hh\:mm")+ " - "+ item.Add(TimeSpan.FromMinutes(slot)).ToString(@"hh\:mm"));
            }

            if (orderType == OrderType.Delivery)
                tomorrowList.RemoveAt(tomorrowList.Count - 1);
            //TimeSpan ts = start;
            //while (ts < end)
            //{
            //    tomorrowList.Add("TOMORROW " + ts.ToString(@"hh\:mm"));
            //    ts = ts.Add(gap);
            //}

            return tomorrowList;
        }
    }
}