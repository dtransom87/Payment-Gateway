using System;
using FluentValidation;
using payment_gateway.Utils;
using payment_gateway.Models.Requests;

namespace payment_gateway.Validators
{
    public class AuthorizationValidator : AbstractValidator<AuthorisationRequest>
    {
        private readonly IDateTimeProvider _datetimeProvider;

        public AuthorizationValidator(IDateTimeProvider datetimeProvider)
        {
            _datetimeProvider = datetimeProvider;

            RuleFor(a => a.CreditCardData.CVV)
                .NotNull()
                .Must(x => x.ToString().Length == 3);

            RuleFor(a => a.CreditCardData.ExpiryMonth)
                .NotEmpty()
                .LessThan(13)
                .WithMessage("Expiry month must be less than 12")
                .GreaterThanOrEqualTo(1);    

            RuleFor(a => a.CreditCardData.ExpiryYear)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);        

            RuleFor(a => a.CreditCardData.CreditCardNumber)
                .NotEmpty()
                .Must(x => x.ToString().Length == 16);     

           RuleFor(cc => cc)
                .Must(cc => IsValidExpiryDate(cc.CreditCardData.ExpiryMonth, (cc.CreditCardData.ExpiryYear)))
                .WithMessage("Expiry Date is in the past"); 
        }

        private bool IsValidExpiryDate(int expiryMonth, int expiryYear)
        {
            if (expiryMonth == 0 || expiryYear == 0 || expiryMonth > 12)
                return false;
            var expiryDate = new DateTime(expiryYear, expiryMonth, 1);
            var dateNow = _datetimeProvider.GetDateTime();
            if (expiryDate < dateNow)
            {
                return false;
            }

            return true;            
        }
    }
}