using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using payment_gateway.Models.Requests;
using payment_gateway.Validators;

namespace payment_gateway.Extensions.Configuration
{
    public static class ValidatorsConfigurationExtensions
    {
        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            return services.AddTransient<IValidator<AuthorisationRequest>, AuthorizationValidator>();
        }
    }
}