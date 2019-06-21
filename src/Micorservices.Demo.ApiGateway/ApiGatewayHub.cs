using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway
{
    public class ApiGatewayHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return Clients.Client(Context.ConnectionId).SendAsync("SetConnectionId", Context.ConnectionId);
        }
    }
}
