using Basket.API.Data.Interfaces;
using StackExchange.Redis;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        public BasketContext(IConnectionMultiplexer redisConnection) => Redis = redisConnection.GetDatabase();

        public IDatabase Redis { get; }
    }
}
