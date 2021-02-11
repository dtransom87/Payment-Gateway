using System;

namespace payment_gateway.Models
{
    public class TransactionUpdateDetails
    {
        public decimal Amount { get; set; }
        public DateTime TransactionCaptureDate { get; set; }
    }
}
