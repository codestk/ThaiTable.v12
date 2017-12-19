using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class SessionHelper
    {
        public static T Read<T>(string variable)
        {
            object value = HttpContext.Current.Session[variable];
            if (value == null)
                return default(T);
            else
                return ((T)value);
        }

        public static void Write(string variable, object value)
        {
            HttpContext.Current.Session[variable] = value;
        }

        public static void Remove(string variable)
        {
            HttpContext.Current.Session.Remove(variable);
        }
    }
}