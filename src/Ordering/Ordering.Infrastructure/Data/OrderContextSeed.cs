using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable StringLiteralTypo

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            var retryForAvailability = retry;

            try
            {
                await orderContext.Database.MigrateAsync();

                if (!await orderContext.Orders.AnyAsync())
                {
                    await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            
            catch (Exception e)
            {
                if (retryForAvailability < 3)
                {
                    var logger = loggerFactory.CreateLogger<OrderContextSeed>();
                    logger.LogError(e.Message);

                    await SeedAsync(orderContext, loggerFactory, ++retryForAvailability);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders() =>
            new List<Order>
            {
                new Order
                {
                    Username = "user",
                    FirstName = "K",
                    LastName = "K",
                    Email = "KK@K.com",
                    Address = "KK Street KK",
                    Country = "GA",
                    State = "TB",
                    ZipCode = "0000",
                    CardName = "KK",
                    CardNumber = "1234432112348765",
                    Expiration = "20/20",
                    CVV = "1234",
                    PaymentMethod = 0
                }
            };
    }
}
