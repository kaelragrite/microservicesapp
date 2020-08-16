using MediatR;
using Ordering.Application.Responses;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    public class GetOrderByUsernameQuery : IRequest<IEnumerable<OrderResponse>>, IRequest<Unit>
    {
        public GetOrderByUsernameQuery(string username) => Username = username;

        public string Username { get; set; }
    }
}
