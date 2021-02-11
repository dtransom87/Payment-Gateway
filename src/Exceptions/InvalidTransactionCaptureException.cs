
using System;

namespace payment_gateway.Services
{   
    public class InvalidTransactionCaptureException : Exception
    {
    public InvalidTransactionCaptureException()
    {

    }

    public InvalidTransactionCaptureException(string errorMessage, string paymentId)
        : base($"{errorMessage} ID: {paymentId}")
        {
  
        }
    }
}