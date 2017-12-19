using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WorldPayDotNet.Utils
{
    public static class CommonUtils
    {
        public static bool AreNullOrEmpty(params string[] stringsToValidate) {
            bool result = false;
            Array.ForEach(stringsToValidate, str => {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

        public static string stripGWInvalidChars(string strIn)
        {
            string[] toReplace = new string[] {"\\","<",">","#","]","["};

            string strOut = strIn;

            foreach (string charToReplace in toReplace) {
                strOut.Replace(charToReplace, "");
            }

            return strOut;
        }


    }
}
