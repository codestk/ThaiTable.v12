using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace WorldPayDotNet.Hosted
{
    /// <summary>
    /// Provides a wrapper around the callback result sent when using SERVER postback
    /// </summary>
    public class CallbackResult
    {
        public CallbackResult(NameValueCollection formVariables, string MD5secretKey, string callbackPW)
        {
            if (formVariables == null)
                throw new ArgumentNullException("formVariables");

            BuildResult(formVariables, MD5secretKey, callbackPW);
        }

        /// <summary>
        /// Your own reference number for this purchase.
        /// Up to 255 Characters.
        /// </summary>
        public string cartId { get; set; }

        /// <summary>
        /// A textual description of this purchase.
        /// Up to 255 Characters.
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// The ID for the transaction.
        /// </summary>
        public string transId { get; set; }

        /// <summary>
        /// The character encoding used to display the payment page to the shopper.
        /// </summary>
        public string charenc { get; set; }

        /// <summary>
        /// The Payment Response password set in the Merchant Interface.
        /// </summary>
        public string callbackPW { get; set; }

        /// <summary>
        /// The Installation Id used.
        /// </summary>
        public string instId { get; set; }

        /// <summary>
        /// A value of 100 specifies a test payment and a value of 0 (zero) specifies a live payment.
        /// </summary>
        public string testMode { get; set; }

        /// <summary>
        /// Specifies the authorisation mode used. The values are "A" for a full auth, or "E" for a pre-auth.
        /// </summary>
        public string authMode { get; set; }

        /// <summary>
        /// A decimal number giving the cost of the purchase in terms of the major currency unit e.g. 12.56 would mean 12 pounds and 56 pence if the currency were GBP (Pounds Sterling).
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        /// An HTML string produced from the amount and currency that were submitted to initiate the payment.
        /// </summary>
        public string amountString { get; set; }

        /// <summary>
        /// A decimal number giving the cost of the purchase in terms of the major currency unit e.g. 12.56 would mean 12 pounds and 56 pence if the currency were GBP (Pounds Sterling).  Note: This is a legacy parameter. Do not use this parameter in server-side scripts.
        /// </summary>
        public string cost { get; set; }

        /// <summary>
        /// 3 letter ISO code for the currency of this payment.
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Amount that the transaction was authorised for, in the currency given as authCurrency.
        /// </summary>
        public string authAmount { get; set; }

        /// <summary>
        /// HTML string produced from authorisation amount and currency.
        /// </summary>
        public string authAmountString { get; set; }

        /// <summary>
        /// Amount that the transaction was authorised for, in the currency given as authCurrency. Note: This is a legacy parameter. Do not use this parameter in any server-side scripts.
        /// </summary>
        public string authCost { get; set; }

        /// <summary>
        /// The currency used for authorisation.
        /// </summary>
        public string authCurrency { get; set; }

        /// <summary>
        /// Result of the transaction - "Y" for a successful payment authorisation, "C" for a cancelled payment.
        /// </summary>
        public string transStatus { get; set; }

        /// <summary>
        /// Time of the transaction in milliseconds since the start of 1970 GMT. This is the standard system date in Java, and is also 1000x the standard C time_t time.
        /// </summary>
        public string transTime { get; set; }

        /// <summary>
        /// A single-character bank authorisation code. This is retained for backward compatibility. 'A' means 'authorised' and is directly equivalent to transStatus='Y'.
        /// </summary>
        public string rawAuthCode { get; set; }

        /// <summary>
        /// A single character describing the result of the comparison of the cardholder country and the issue country of the card used by the shopper (where available). Note that this parameter is retained for backward compatibility - equivalent information is now provided as part of the AVS results. The result possible values are:
        /// Y - match
        /// N - no match (i.e. mismatch)
        /// B - comparison not available
        /// I - contact country not supplied
        /// S - card issue country not available
        /// </summary>
        public string countryMatch { get; set; }

        /// <summary>
        /// The text received from the bank summarising the different states listed below:
        /// cardbe.msg.authorised - Make Payment (test or live)
        /// trans.cancelled - Cancel Purchase (test or live) 
        /// </summary>
        public string rawAuthMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string msgType { get; set; }

        /// <summary>
        /// A 4-character string giving the results of 4 internal fraud-related checks. The characters respectively give the results of the following checks:
        /// - 1st character - Card Verification Value check
        /// - 2nd character - postcode AVS check
        /// - 3rd character - address AVS check
        /// - 4th character - country comparison check (see also countryMatch)
        /// The possible values for each result character are:
        /// - 0 - Not supported
        /// - 1 - Not checked
        /// - 2 - Matched
        /// - 4 - Not matched
        /// - 8 - Partially matched
        /// </summary>
        public string AVS { get; set; }

        /// <summary>
        /// The type of payment method used by the shopper.
        /// </summary>
        public string cardType { get; set; }

        /// <summary>
        /// The IP address from which the purchase token was submitted.
        /// </summary>
        public string ipAddress { get; set; }

        /// <summary>
        /// The Shopper's full name, including any title, personal name and family name. Note: If your purchase token does not contain a name value, the name that the cardholder enters on the payment page will be returned to you.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The first line of the shopper's address. Separators (including new line) used in this parameter are encoded as ASCII characters.
        /// </summary>
        public string address1 { get; set; }

        /// <summary>
        /// The second line of the shopper's address.
        /// </summary>
        public string address2 { get; set; }

        /// <summary>
        /// The third line of the shopper's address.
        /// </summary>
        public string address3 { get; set; }

        /// <summary>
        /// Shopper’s city or town.
        /// </summary>
        public string town { get; set; }

        /// <summary>
        /// Shopper’s country/region/state or area.
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// Shopper's postcode.
        /// </summary>
        public string postcode { get; set; }

        /// <summary>
        /// Shopper's country, as 2 character ISO code, uppercase.
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// The full name of the country, derived from the country code submitted or supplied by the shopper in the language used by the shopper on the payment page.
        /// </summary>
        public string countryString { get; set; }

        /// <summary>
        /// Shopper's telephone number.
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// Shopper's fax number.
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// Shopper's email address.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Shopper's delivery name. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvName { get; set; }

        /// <summary>
        /// Shopper's delivery address1. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvAddress1 { get; set; }

        /// <summary>
        /// Shopper's delivery address 2. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvAddress2 { get; set; }

        /// <summary>
        /// Shopper's delivery address 3. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvAddress3 { get; set; }

        /// <summary>
        /// Shopper's delivery town or city. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvTown { get; set; }

        /// <summary>
        /// Shopper's delivery county/state/region. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvRegion { get; set; }

        /// <summary>
        /// Shopper's delivery postcode. Note: The withDelivery parameter must be submitted in the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvPostcode { get; set; }

        /// <summary>
        /// Shopper's delivery country, as 2 character ISO code, uppercase. Note: The withDelivery parameter must be submitted in  the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvCountry { get; set; }

        /// <summary>
        /// The full name of the country, derived from the country code submitted or supplied by the shopper for the delivery address in the language used by the shopper on the payment page.
        /// Note: The withDelivery parameter must be submitted in  the purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string delvCountryString { get; set; }

        /// <summary>
        /// Validation Hash passed back to the website - used to validate the callback.
        /// </summary>
        public string MC_callbacksignature { get; set; }

        protected void BuildResult(NameValueCollection formVariables, string MD5secretKey, string callbackPW)
        {
            this.cartId = formVariables["cartId"];
            this.desc = formVariables["desc"];
            this.transId = formVariables["transId"];
            this.charenc = formVariables["charenc"];

            this.callbackPW = formVariables["callbackPW"];
            this.instId = formVariables["instId"];
            this.testMode = formVariables["testMode"];
            this.authMode = formVariables["authMode"];

            this.amount = formVariables["amount"];
            this.amountString = formVariables["amountString"];
            this.cost = formVariables["cost"];
            this.currency = formVariables["currency"];

            this.authAmount = formVariables["authAmount"];
            this.authAmountString = formVariables["authAmountString"];
            this.authCost = formVariables["authCost"];
            this.authCurrency = formVariables["authCurrency"];

            this.transStatus = formVariables["transStatus"];
            this.transTime = formVariables["transTime"];
            this.rawAuthCode = formVariables["rawAuthCode"];
            this.countryMatch = formVariables["countryMatch"];
            this.rawAuthMessage = formVariables["rawAuthMessage"];
            this.msgType = formVariables["msgType"];
            this.AVS = formVariables["AVS"];
            this.cardType = formVariables["cardType"];
            this.ipAddress = formVariables["ipAddress"];

            this.name = formVariables["name"];
            this.address1 = formVariables["address1"];
            this.address2 = formVariables["address2"];
            this.address3 = formVariables["address3"];
            this.region = formVariables["region"];
            this.postcode = formVariables["postcode"];
            this.country = formVariables["country"];
            this.countryString = formVariables["countryString"];
            this.tel = formVariables["tel"];
            this.fax = formVariables["fax"];
            this.email = formVariables["email"];

            this.delvName = formVariables["delvName"];
            this.delvAddress1 = formVariables["delvAddress1"];
            this.delvAddress2 = formVariables["delvAddress2"];
            this.delvAddress3 = formVariables["delvAddress3"];
            this.delvTown = formVariables["delvTown"];
            this.delvRegion = formVariables["delvRegion"];
            this.delvPostcode = formVariables["delvPostcode"];
            this.delvCountry = formVariables["delvCountry"];
            this.delvCountryString = formVariables["delvCountryString"];

            this.MC_callbacksignature = formVariables["MC_callbacksignature"];

            var hashInputs = new StringBuilder();
            hashInputs.Append(MD5secretKey);
            hashInputs.Append(":");
            hashInputs.Append(this.currency);
            hashInputs.Append(":");
            hashInputs.Append(this.amount);
            hashInputs.Append(":");
            hashInputs.Append(this.testMode);
            hashInputs.Append(":");
            hashInputs.Append(this.instId);

            byte[] hashDigest = new MD5CryptoServiceProvider().ComputeHash(WorldPayDotNet.Hosted.HostedPaymentProcessor.StringToByteArray(hashInputs.ToString()));
            string newhash = WorldPayDotNet.Hosted.HostedPaymentProcessor.ByteArrayToHexString(hashDigest);        

            //Check if Callback Password matches
            if (newhash != this.MC_callbacksignature || this.callbackPW != callbackPW)
            {
                throw new Exception("Callback hash validation failed.");
            }
        }
    }
}
