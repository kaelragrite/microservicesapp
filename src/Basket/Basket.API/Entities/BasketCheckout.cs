namespace Basket.API.Entities
{
    public class BasketCheckout
    {
        public string Username { get; set; }

        public decimal TotalPrice { get; set; }

        // BillingAddress
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string Expiration { get; set; }

        // ReSharper disable once InconsistentNaming
        public string CVV { get; set; }

        public int PaymentMethod { get; set; }
    }
}
