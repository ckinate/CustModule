using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Infrastructure.Integration.Caching;
using Fintrak.CustomerPortal.Infrastructure.Integration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fintrak.CustomerPortal.Infrastructure.Integration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBankIntegrationService, BankIntegrationService>();
			services.AddScoped<ITokenIntegrationService, TokenIntegrationService>();
			services.AddScoped<ICustomerIntegrationService, CustomerIntegrationService>();
            services.AddScoped<ICentralPayIntegrationService, CentralPayIntegrationService>();

            // Add memory cache services
            services.AddMemoryCache();
            services.AddRedisCache(configuration);

            return services;
        }
    }
}
