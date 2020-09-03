using System;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Producers;

namespace Basket.API.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly EventBusRabbitMqProducer _eventBus;

        public BasketController(IMapper mapper, IBasketRepository basketRepository, EventBusRabbitMqProducer eventBus)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string username) => Ok(await _basketRepository.GetBasket(username));

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basket) => Ok(await _basketRepository.UpdateBasket(basket));

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username) => Ok(await _basketRepository.DeleteBasket(username));

        [HttpPost, Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.Username);
            if (basket is null) return BadRequest();

            var basketRemoved = await _basketRepository.DeleteBasket(basket.Username);
            if (!basketRemoved) return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.CalculateTotalPrice();

            _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);

            return Accepted();
        }
    }
}
