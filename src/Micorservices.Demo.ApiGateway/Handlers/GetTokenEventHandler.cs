using System.Threading.Tasks;
using Microservices.Demo.Core.Events;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Micorservices.Demo.ApiGateway.Handlers
{
    public class GetTokenEventHandler : IEventHandler<GetTokenEvent>
    {
        private readonly IHubContext<ApiGatewayHub> _hubContext;

        public GetTokenEventHandler(IHubContext<ApiGatewayHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task HandleAsync(GetTokenEvent @event)
        {
            await this._hubContext.Clients.Client(@event.ConnectionId).SendAsync("GetTokenResponse", new { status = StatusCodes.Status200OK, token = @event.Token });
        }
    }
}
