using Microsoft.Extensions.DependencyInjection;
using payment_gateway.Models;
using payment_gateway.Repositories;

namespace payment_gateway.Extensions.Configuration
{
    public static class RepositoryConfigurationExtensions
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            return services.AddSingleton<IRepository<Payment>, PaymentRepository>();
        }
    }
}