using Microservices.Demo.Core.Events;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway.Handlers
{
    public class GetTokenRejectedEventHandler : IEventHandler<GetTokenRejectedEvent>
    {
        private readonly IHubContext<ApiGatewayHub> _hubContext;

        public GetTokenRejectedEventHandler(IHubContext<ApiGatewayHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task HandleAsync(GetTokenRejectedEvent @event)
        {
            await this._hubContext.Clients.Client(@event.ConnectionId).SendAsync("GetTokenResponse", new { status = StatusCodes.Status500InternalServerError, error = @event });
        }
    }
}
