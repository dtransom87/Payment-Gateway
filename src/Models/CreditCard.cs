namespace payment_gateway.Models
{
    public class CreditCard
    {
        public long CreditCardNumber { get; set; }

        public int CVV { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }
    }
}
