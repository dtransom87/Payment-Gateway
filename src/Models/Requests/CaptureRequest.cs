namespace payment_gateway.Models.Requests
{
    public class UpdatePaymentRequest
    {
        public string PaymentId { get; set; }
        public TransactionUpdateDetails TransactionDetails { get; set; }
    }
}
