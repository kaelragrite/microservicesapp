using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventBusRabbitMqConsumer(IRabbitMqConnection connection, IMediator mediator, IMapper mapper)
        {
            _connection = connection;
            _mediator = mediator;
            _mapper = mapper;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(EventBusConstants.BasketCheckoutQueue, false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(EventBusConstants.BasketCheckoutQueue, true, consumer);
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey != EventBusConstants.BasketCheckoutQueue) return;

            var message = Encoding.UTF8.GetString(e.Body.Span);
            var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

            var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
            await _mediator.Send(command);
        }

        public void Disconnect() => _connection.Dispose();
    }
}
