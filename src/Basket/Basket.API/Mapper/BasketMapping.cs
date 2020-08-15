using AutoMapper;
using Basket.API.Entities;
using EventBusRabbitMQ.Events;

namespace Basket.API.Mapper
{
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
