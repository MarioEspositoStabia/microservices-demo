using Microservices.Demo.Core.Events;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway.Handlers
{
    public class CreateUserRejectedEventHandler : IEventHandler<CreateUserRejectedEvent>
    {
        private readonly IHubContext<ApiGatewayHub> _hubContext;

        public CreateUserRejectedEventHandler(IHubContext<ApiGatewayHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task HandleAsync(CreateUserRejectedEvent @event)
        {
            await this._hubContext.Clients.Client(@event.ConnectionId).SendAsync("GetCreateUserResponse", new { status = StatusCodes.Status500InternalServerError, error = @event });
        }
    }
}