using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context) => _context = context;

        public async Task<BasketCart> GetBasket(string username)
        {
            var basket = await _context.Redis.StringGetAsync(username);

            return basket.IsNullOrEmpty 
                ? null 
                : JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var updated = await _context.Redis.StringSetAsync(basket.Username, JsonConvert.SerializeObject(basket));

            return !updated 
                ? null 
                : await GetBasket(basket.Username);
        }

        public async Task<bool> DeleteBasket(string username) => await _context.Redis.KeyDeleteAsync(username);
    }
}
