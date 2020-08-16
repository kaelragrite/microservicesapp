using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mappers;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutOrderHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.AddAsync(OrderMapper.Mapper.Map<Order>(request));

            return OrderMapper.Mapper.Map<OrderResponse>(order);
        }
    }
}
