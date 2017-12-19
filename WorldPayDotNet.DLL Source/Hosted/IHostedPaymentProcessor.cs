using System;
using WorldPayDotNet.Common;
namespace WorldPayDotNet.Hosted
{
    public interface IHostedPaymentProcessor {
        void SubmitTransaction(HostedTransactionRequest request, string MD5secretKey);
    }
}
