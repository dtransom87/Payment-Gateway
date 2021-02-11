namespace payment_gateway.Models.Requests
{
    public class AuthorisationRequest
    {
        public CreditCard CreditCardData { get; set; }
        public TransactionDetails TransactionDetails { get; set; }
    }
}
