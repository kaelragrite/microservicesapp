using MediatR;
using Ordering.Application.Mappers;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUsernameHandler : IRequestHandler<GetOrderByUsernameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByUsernameHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderByUsernameQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByUsername(request.Username);

            return OrderMapper.Mapper.Map<IEnumerable<OrderResponse>>(orders);
        }
    }
}
