using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Globalization;
using ThaiTable.Models;
using System.Web.Mvc;

public class Worldpay
{
    public static string ExpressCheckout(WorldpayViewModel order)
    {
        NameValueCollection values = new NameValueCollection();

        values["instId"] = "1018224";
        values["cartId"] = order.OrderID;
        values["amount"] = order.Total;
        values["currency"] = "GBP";
        values["desc"] = order.Description;
        values["testMode"] = "100";
        values["MC_callback"] = "http://Ns3.sbcserver.com/WorldPay/WorldPayCallback";
        values["name"] = order.CustomerName;
        values["address"] = order.Address;
        values["postcode"] = order.PostCode;

        return Submit(values);

        //values = Submit(values);

        ////string ack = values["ACK"].ToLower();

        ////if (ack == "success" || ack == "successwithwarning")
        ////{
        //    return new WorldpayRedirect
        //    {
        //        Token = values["TOKEN"],
        //        Url = String.Format("https://select.worldpay.com/wcc/purchase?{0}",
        //           values["TOKEN"])
        //    };
        //}
        //else
        //{
        //    throw new Exception(values["L_LONGMESSAGE0"]);
        //}
    }

    private static string Submit(NameValueCollection values)
    {
        string data = String.Join("&", values.Cast<string>()
          .Select(key => String.Format("{0}={1}", key, HttpUtility.UrlEncode(values[key]))));

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://select.worldpay.com/wcc/purchase");

        request.Method = "POST";
        request.ContentLength = data.Length;

        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(data);
        }

        using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
        {
            return reader.ReadToEnd();
        }
    }
}