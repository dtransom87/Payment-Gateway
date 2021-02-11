using Microsoft.Extensions.DependencyInjection;
using payment_gateway.Models;
using payment_gateway.Repositories;
using payment_gateway.Services;

namespace payment_gateway.Extensions.Configuration
{
    public static class ServicesConfigurationExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services.AddSingleton<IPaymentService, PaymentService>();
        }
    }
}