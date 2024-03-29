using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Caching
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration["redis:connectionString"];
            var host = configuration["Redis:Host"];
            var port = configuration["Redis:Port"];
            //var userName = configuration["Redis:UserName"];
            //var password = configuration["Redis:Password"];
            var abortOnConnectFail = bool.Parse(configuration["Redis:AbortOnConnectFail"].ToString());
            var enableSsl = bool.Parse(configuration["Redis:EnableSsl"].ToString());

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions();

                options.ConfigurationOptions.EndPoints.Add($"{host}:{port}");
                //options.ConfigurationOptions.Password = password;
                //options.ConfigurationOptions.User = userName;

                options.ConfigurationOptions.AbortOnConnectFail = abortOnConnectFail;
                //options.ConfigurationOptions.SyncTimeout = 10000;
                options.ConfigurationOptions.Ssl = enableSsl;
                if (enableSsl)
                {
                    options.ConfigurationOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                }
            });

            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}
