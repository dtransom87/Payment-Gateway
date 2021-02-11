
using System;

namespace payment_gateway.Services
{   
    public class PaymentGatewayServiceException : Exception
    {
    public PaymentGatewayServiceException()
    {

    }

    public PaymentGatewayServiceException(string errorMessage)
        : base(errorMessage)
        {
  
        }
    }
}