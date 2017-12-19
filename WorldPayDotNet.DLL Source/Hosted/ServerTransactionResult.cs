using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using WorldPayDotNet.Common;
using WorldPayDotNet.Utils;

namespace WorldPayDotNet.Hosted
{
    public class ServerTransactionResult
    {
        public ServerTransactionResult(NameValueCollection formVariables)
        {
            if (formVariables == null)
                throw new ArgumentNullException("formVariables");

            BuildResult(formVariables);
        }

        public string HashDigest { get; set; }
        public string MerchantID { get; set; }
        public TransactionStatus StatusCode { get; set; }
        public string Message { get; set; }
        public TransactionStatus PreviousStatusCode { get; set; }
        public string PreviousMessage { get; set; }
        public string CrossReference { get; set; }
        public string AddressNumericCheckResult { get; set; }
        public string PostCodeCheckResult { get; set; }
        public string CV2CheckResult { get; set; }
        public string ThreeDSecureAuthenticationCheckResult { get; set; }
        public string CardType { get; set; }
        public string CardClass { get; set; }
        public string CardIssuer { get; set; }
        public string CardIssuerCountryCode { get; set; }
        public int Amount { get; set; }
        public int CurrencyCode { get; set; }
        public string OrderID { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDateTime { get; set; }
        public string OrderDescription { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public int CountryCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public NameValueCollection ReturnedFormVariables = new NameValueCollection();

        public bool Successful
        {
            get { return this.StatusCode == TransactionStatus.Successful; }
        }

        protected void BuildResult(NameValueCollection formVariables)
        {
            this.HashDigest = formVariables["HashDigest"];
            this.MerchantID = formVariables["MerchantID"];
            this.StatusCode = formVariables["StatusCode"].ToEnum(TransactionStatus.NotSpecified);
            this.Message = formVariables["Message"];
            this.PreviousStatusCode = formVariables["PreviousStatusCode"].ToEnum(TransactionStatus.NotSpecified);
            this.PreviousMessage = formVariables["PreviousMessage"];
            this.CrossReference = formVariables["CrossReference"];

            this.AddressNumericCheckResult = formVariables["AddressNumericCheckResult"];
            this.PostCodeCheckResult = formVariables["PostCodeCheckResult"];
            this.CV2CheckResult = formVariables["CV2CheckResult"];
            this.ThreeDSecureAuthenticationCheckResult = formVariables["ThreeDSecureAuthenticationCheckResult"];

            this.CardType = formVariables["CardType"];
            this.CardClass = formVariables["CardClass"];
            this.CardIssuer = formVariables["CardIssuer"];
            this.CardIssuerCountryCode = formVariables["CardIssuerCountryCode"];

            this.Amount = int.Parse(formVariables["Amount"]);
            this.CurrencyCode = int.Parse(formVariables["CurrencyCode"]);
            this.OrderID = formVariables["OrderID"];
            this.TransactionType = formVariables["TransactionType"];
            this.TransactionDateTime = formVariables["TransactionDateTime"];
            this.OrderDescription = formVariables["OrderDescription"];

            this.CustomerName = formVariables["CustomerName"];
            this.Address1 = formVariables["Address1"];
            this.Address2 = formVariables["Address2"];
            this.Address3 = formVariables["Address3"];
            this.Address4 = formVariables["Address4"];
            this.City = formVariables["City"];
            this.State = formVariables["State"];
            this.PostCode = formVariables["PostCode"];
            this.CountryCode = int.Parse(formVariables["CountryCode"]);

            this.EmailAddress = formVariables["EmailAddress"];
            this.PhoneNumber = formVariables["PhoneNumber"];

            foreach (string key in formVariables)
            {
                this.ReturnedFormVariables.Add(key, formVariables[key]);
            }
        }
    }
}
