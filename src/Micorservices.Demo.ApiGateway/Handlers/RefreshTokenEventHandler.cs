using Microservices.Demo.Core.Events;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway.Handlers
{
    public class RefreshTokenEventHandler : IEventHandler<RefreshTokenEvent>
    {
        private readonly IHubContext<ApiGatewayHub> _hubContext;

        public RefreshTokenEventHandler(IHubContext<ApiGatewayHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task HandleAsync(RefreshTokenEvent @event)
        {
            await this._hubContext.Clients.Client(@event.ConnectionId).SendAsync("RefreshTokenResponse", new { status = StatusCodes.Status200OK, @event.Token });
        }
    }
}
