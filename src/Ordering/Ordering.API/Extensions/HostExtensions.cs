using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Data;
using System;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost CreateAndSeedDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var orderContext = services.GetRequiredService<OrderContext>();
                _ = OrderContextSeed.SeedAsync(orderContext, loggerFactory);
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(e.Message);
            }

            return host;
        }
    }
}
