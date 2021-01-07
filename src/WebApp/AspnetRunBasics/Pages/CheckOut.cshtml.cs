using System.Threading.Tasks;
using AspnetRunBasics.ApiCollection.Interfaces;
using AspnetRunBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketApi _basketApi;
        private readonly IOrderApi _orderApi;

        public CheckOutModel(IBasketApi basketApi, IOrderApi orderApi)
        {
            _basketApi = basketApi;
            _orderApi = orderApi;
        }

        [BindProperty]
        public BasketCheckoutModel Order { get; set; }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            const string username = "swn";
            Cart = await _basketApi.GetBasket(username);

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            if (!ModelState.IsValid) return Page();
            
            const string username = "swn";
            Cart = await _basketApi.GetBasket(username);

            Order.Username = username;
            Order.TotalPrice = Cart.TotalPrice;

            await _basketApi.CheckoutBasket(Order);
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}