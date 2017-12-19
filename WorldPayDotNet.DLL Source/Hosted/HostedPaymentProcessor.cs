using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.Security.Cryptography;
using WorldPayDotNet.Utils;
using WorldPayDotNet.Common;

namespace WorldPayDotNet.Hosted
{
    public class HostedPaymentProcessor : IHostedPaymentProcessor
    {
        private readonly HttpContext _context;

        /// <summary>
        /// Creates a new instance of the payment processor
        /// </summary>
        /// <param name="merchantId">The merchant ID that corresponds to the gateway account the transaction will be run through.</param>
        /// <param name="context">The current http context</param>
        public HostedPaymentProcessor(HttpContext context) {
            _context = context;
        }

        /// <summary>
        /// Submits a payment request to the hosted payment page
        /// </summary>
        /// <param name="request">The request to submit</param>
        /// <param name="merchantPassword">The merchant password that corresponds to the gateway account the transaction will be run through.</param>
        /// <param name="preSharedKey">The merchant gateway account pre shared key</param>
        /// <param name="postUrl">The url of the hosted payment page</param>
        public void SubmitTransaction(HostedTransactionRequest request, string MD5secretKey)
        {
            if (CommonUtils.AreNullOrEmpty(MD5secretKey))
                throw new ArgumentNullException();
            
            if (request == null)
                throw new ArgumentNullException("request");

            var requestInputs = request.ToNameValueCollection();
            var postUrl = "";

            if (request.testMode == 100)
            {
                postUrl = "https://secure-test.worldpay.com/wcc/purchase";
            }
            else
            {
                postUrl = "https://secure.worldpay.com/wcc/purchase";
            }
            
            // ready to post - just return the NameValue Collection
            var remotePost = new RemotePost(_context, postUrl, FormMethod.POST);

            var callbackhashInputs = new StringBuilder();
            callbackhashInputs.Append(MD5secretKey);
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(request.currency);
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(request.amount.ToString("#0.00"));
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(request.testMode);
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(request.instId);

            var signaturehashInputs = new StringBuilder();
            signaturehashInputs.Append(MD5secretKey);
            signaturehashInputs.Append(":");
            signaturehashInputs.Append(request.currency);
            signaturehashInputs.Append(":");
            signaturehashInputs.Append(request.amount);
            signaturehashInputs.Append(":");
            signaturehashInputs.Append(request.testMode);
            signaturehashInputs.Append(":");
            signaturehashInputs.Append(request.instId);

            byte[] callbackhashDigest = new MD5CryptoServiceProvider().ComputeHash(StringToByteArray(callbackhashInputs.ToString()));

            byte[] signaturehashDigest = new MD5CryptoServiceProvider().ComputeHash(StringToByteArray(signaturehashInputs.ToString()));

            remotePost.AddInput("signature", ByteArrayToHexString(signaturehashDigest));
            remotePost.AddInput("MC_callbacksignature", ByteArrayToHexString(callbackhashDigest));

            // add the rest of the form variables
            foreach (var k in requestInputs.AllKeys)
            {
                remotePost.AddInput(k, requestInputs.GetValues(k)[0]);
            }

            remotePost.Post("CardsavePaymentForm");
        }

        public static byte[] StringToByteArray(string source, bool useASCII = true)
        {
            Encoding e;
            if (useASCII)
                e = new ASCIIEncoding();
            else
                e = new UTF8Encoding();
            return e.GetBytes(source);
        }

        public static string ByteArrayToHexString(byte[] source)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sb.Append(source[i].ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Performs basic validation of the transaction result (you should also implement your own e.g. check amounts against order)
        /// </summary>
        /// <param name="result">Transaction result</param>
        public void ValidateResult(ServerTransactionResult result, String MerchantId, String MerchantPassword, String PreSharedKey)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            HashMethod hashMethod = HashMethod.SHA1;
            nameValueCollection.Add("PreSharedKey", PreSharedKey);
            nameValueCollection.Add("MerchantID", MerchantId);
            nameValueCollection.Add("Password", MerchantPassword);
            nameValueCollection.Add("StatusCode", Convert.ToInt32(result.StatusCode));
            nameValueCollection.Add("Message", result.Message);
            if (result.StatusCode == TransactionStatus.DuplicateTransaction)
            {
                nameValueCollection.Add("PreviousStatusCode", Convert.ToInt32(result.PreviousStatusCode));
            }
            else
            {
                nameValueCollection.Add("PreviousStatusCode", "");
            }
            nameValueCollection.Add("PreviousMessage", result.PreviousMessage);
            nameValueCollection.Add("CrossReference", result.CrossReference);
            nameValueCollection.Add("AddressNumericCheckResult", result.AddressNumericCheckResult);
            nameValueCollection.Add("PostCodeCheckResult", result.PostCodeCheckResult);
            nameValueCollection.Add("CV2CheckResult", result.CV2CheckResult);
            nameValueCollection.Add("ThreeDSecureAuthenticationCheckResult", result.ThreeDSecureAuthenticationCheckResult);
            nameValueCollection.Add("CardType", result.CardType);
            nameValueCollection.Add("CardClass", result.CardClass);
            nameValueCollection.Add("CardIssuer", result.CardIssuer);
            nameValueCollection.Add("CardIssuerCountryCode", result.CardIssuerCountryCode);
            nameValueCollection.Add("Amount", result.Amount);
            nameValueCollection.Add("CurrencyCode", Convert.ToString(result.CurrencyCode));
            nameValueCollection.Add("OrderID", result.OrderID);
            nameValueCollection.Add("TransactionType", result.TransactionType);
            nameValueCollection.Add("TransactionDateTime", Convert.ToString(result.TransactionDateTime));
            nameValueCollection.Add("OrderDescription", result.OrderDescription);
            nameValueCollection.Add("CustomerName", result.CustomerName);
            nameValueCollection.Add("Address1", result.Address1);
            nameValueCollection.Add("Address2", result.Address2);
            nameValueCollection.Add("Address3", result.Address3);
            nameValueCollection.Add("Address4", result.Address4);
            nameValueCollection.Add("City", result.City);
            nameValueCollection.Add("State", result.State);
            nameValueCollection.Add("PostCode", result.PostCode);
            nameValueCollection.Add("CountryCode", Convert.ToString(result.CountryCode));
            nameValueCollection.Add("EmailAddress", result.EmailAddress);
            nameValueCollection.Add("PhoneNumber", result.PhoneNumber);
            bool flag = false;
            string queryString = nameValueCollection.ToQueryString("&", false, flag);
            string str = HashUtil.ComputeHashDigest(queryString, PreSharedKey, hashMethod);
            if (result.HashDigest != str)
            {
                throw new Exception("Hash Check Failed");
            }
        }

        /// <summary>
        /// Creates a response message confirming delivery of transaction result
        /// </summary>
        /// <param name="status">Result of delivery (note this is to confirm that you have received the result, not the result itself)</param>
        /// <param name="message">Optional message for example, any exceptions that may have occurred</param>
        /// <returns>String</returns>
        public string CreateServerResponseString(TransactionStatus status, string message = "")
        {

            var response = new NameValueCollection();
            response.Add("StatusCode", (int)status);
            if (!string.IsNullOrEmpty(message))
            {
                response.Add("Message", message);
            }

            return response.ToQueryString();
        }
    }
}
