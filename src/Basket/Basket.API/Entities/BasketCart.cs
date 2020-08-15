using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public BasketCart() { }

        public BasketCart(string username) => Username = username;

        public string Username { get; set; }

        public List<BasketCartItem> Items { get; set; } = new List<BasketCartItem>();


        public decimal CalculateTotalPrice() => Items.Sum(x => x.Price * x.Quantity);
    }
}
