using Microsoft.Extensions.DependencyInjection;
using payment_gateway.Utils;

namespace payment_gateway.Extensions.Configuration
{
    public static class UtilsConfigurationExtensions
    {
        public static IServiceCollection ConfigureUtils(this IServiceCollection services)
        {
            return services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }
    }
}