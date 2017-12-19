using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using WorldPayDotNet.Common;
using WorldPayDotNet.Utils;

namespace WorldPayDotNet.Hosted
{
    /// <summary>
    /// Represents a transaction request for WorldPays's hosted payment page
    /// </summary>
    public class HostedTransactionRequest
    {
        public HostedTransactionRequest()
        {
            // defaults
        }
        
        /// <summary>
        /// Your Installation Id.
        /// </summary>
        public int instId { get; set; }

        /// <summary>
        /// A decimal number giving the cost of the purchase in terms of the major currency unit e.g. 12.56 would mean 12 pounds and 56 pence if the currency were GBP (Pounds Sterling).
        /// Note that the decimal separator must be a dot (.), regardless of the typical language convention for the chosen currency.
        /// The decimal separator does not need to be included if the amount is an integral multiple of the major currency unit.
        /// Do not include other separators, for example between thousands.
        /// </summary>
        public double amount { get; set; }

        /// <summary>
        /// Your own reference number for this purchase.
        /// It is returned to you along with the authorisation results by whatever method you have chosen for being informed (email and / or Payment Notifications).
        /// Up to 255 Characters.
        /// </summary>
        public string cartId { get; set; }

        /// <summary>
        /// A textual description of this purchase, up to 255 characters. This is used in web-pages, statements and emails for yourself and the shopper.
        /// Up to 255 Characters.
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// 3 letter ISO code for the currency of this payment.
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// The shopper's billing full name, including any title, personal name and family name.
        /// Up to 40 Characters.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The first line of the shopper's billing address.
        /// Up to 84 Characters.
        /// </summary>
        public string address1 { get; set; }

        /// <summary>
        /// The second line of the shopper's billing address.
        /// Up to 84 Characters.
        /// </summary>
        public string address2 { get; set; }

        /// <summary>
        /// The third line of the shopper's billing address.
        /// Up to 84 Characters.
        /// </summary>
        public string address3 { get; set; }

        /// <summary>
        /// The shopper's billing town or city.
        /// Up to 30 Characters.
        /// </summary>
        public string town { get; set; }

        /// <summary>
        /// The shopper's billing region/county/state.
        /// Up to 30 Characters.
        /// </summary>
        public string region { get; set; }        

        /// <summary>
        /// The shopper's billing postcode.
        /// </summary>
        public string postcode { get; set; }

        /// <summary>
        /// The shopper's billing country, as 2 character ISO code, uppercase.
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// The shopper's delivery full name, including any title, personal name and family name.
        /// Up to 40 Characters.
        /// </summary>
        public string delvName { get; set; }

        /// <summary>
        /// The first line of the shopper's delivery address.
        /// Up to 84 Characters.
        /// </summary>
        public string delvAddress1 { get; set; }

        /// <summary>
        /// The second line of the shopper's delivery address.
        /// Up to 84 Characters.
        /// </summary>
        public string delvAddress2 { get; set; }

        /// <summary>
        /// The third line of the shopper's delivery address.
        /// Up to 84 Characters.
        /// </summary>
        public string delvAddress3 { get; set; }

        /// <summary>
        /// The shopper's delivery town or city.
        /// Up to 30 Characters.
        /// </summary>
        public string delvTown { get; set; }

        /// <summary>
        /// The shopper's delivery region/county/state.
        /// Up to 30 Characters.
        /// </summary>
        public string delvRegion { get; set; }

        /// <summary>
        /// The shopper's delivery postcode.
        /// </summary>
        public string delvPostcode { get; set; }

        /// <summary>
        /// The shopper's delivery country, as 2 character ISO code, uppercase.
        /// </summary>
        public string delvCountry { get; set; }

        /// <summary>
        /// The shopper's telephone number.
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// The shopper's fax number.
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// The shopper's email address.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// The name of one of your uploaded files, which will be used to format the result.
        /// </summary>
        public string resultFile { get; set; }
        
        /// <summary>
        /// This specifies the authorisation mode to use.
        /// </summary>
        public TransactionType authMode { get; set; }

        /// <summary>
        /// Specifies that this is a test payment or not.
        /// </summary>
        public int testMode { get; set; }

        /// <summary>
        /// If set to True, this causes the currency drop down to be hidden, so fixing the currency that the shopper must purchase in.
        /// </summary>
        public Boolean hideCurrency { get; set; }

        /// <summary>
        /// If set to True, this causes the contact details to be displayed in non-editable format. You must ensure that all mandatory contact details are submitted in your initial request.
        /// </summary>
        public Boolean fixContact { get; set; }

        /// <summary>
        /// If set to True, this causes the contact details to be hidden. You must ensure that all mandatory contact details are submitted in your initial request.
        /// </summary>
        public Boolean hideContact { get; set; }

        /// <summary>
        /// If set to True, passes the delivery address to the gateway.
        /// </summary>
        public Boolean withDelivery { get; set; }

        /// <summary>
        /// Callback URL.
        /// </summary>
        public string MC_callback { get; set; }

        /// <summary>
        /// Converts the transaction request into a NameValueCollection ready for posting to the hosted payment page
        /// </summary>
        /// <returns>NameValueCollection</returns>
        public NameValueCollection ToNameValueCollection()
        {
            var collection = new NameValueCollection();
            collection.AddProperty(this, r => r.instId);
            collection.AddProperty(this, r => r.amount);
            collection.AddProperty(this, r => r.cartId);
            collection.AddProperty(this, r => r.desc);
            collection.AddProperty(this, r => r.currency);
            collection.AddProperty(this, r => r.name);
            collection.AddProperty(this, r => r.address1);
            collection.AddProperty(this, r => r.address2);
            collection.AddProperty(this, r => r.address3);
            collection.AddProperty(this, r => r.town);
            collection.AddProperty(this, r => r.region);
            collection.AddProperty(this, r => r.postcode);
            collection.AddProperty(this, r => r.country);
            
            collection.AddProperty(this, r => r.delvName);
            collection.AddProperty(this, r => r.delvAddress1);
            collection.AddProperty(this, r => r.delvAddress2);
            collection.AddProperty(this, r => r.delvAddress3);
            collection.AddProperty(this, r => r.delvTown);
            collection.AddProperty(this, r => r.delvRegion);
            collection.AddProperty(this, r => r.delvPostcode);
            collection.AddProperty(this, r => r.delvCountry);

            collection.AddProperty(this, r => r.tel);
            collection.AddProperty(this, r => r.fax);
            collection.AddProperty(this, r => r.email);
            collection.AddProperty(this, r => r.resultFile);
            collection.AddProperty(this, r => r.authMode);
            collection.AddProperty(this, r => r.testMode);
            collection.AddProperty(this, r => r.hideCurrency);
            collection.AddProperty(this, r => r.fixContact);
            collection.AddProperty(this, r => r.hideContact);
            collection.AddProperty(this, r => r.withDelivery);

            collection.AddProperty(this, r => r.MC_callback);

            return collection;
        }
    }
}
