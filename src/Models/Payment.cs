using System;
using System.Collections.Generic;

namespace payment_gateway.Models
{
    public class Payment : EntityBase
    {
        public CreditCard CreditCardDetails { get; set; }

        public TransactionDetails TransactionDetails { get; set; }

        public IList<TransactionUpdateDetails> TransactionCaptureDetails { get; set; }

        public bool IsVoid { get; set; }

        public IList<TransactionUpdateDetails> TransactionRefundDetails { get; set; }

        public DateTime PaymentCompletedDate { get; set; }
    }
}
