using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC.Utils;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Threading.Tasks;

namespace Micorservices.Demo.ApiGateway.Handlers
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly IHubContext<ApiGatewayHub> _hubContext;
        private readonly IEmailHandler _emailHandler;
        private readonly StringBuilder _stringBuilder = new StringBuilder(500);

        public UserCreatedEventHandler(IHubContext<ApiGatewayHub> hubContext, IEmailHandler emailHandler)
        {
            this._hubContext = hubContext;
            this._emailHandler = emailHandler;
        }

        public async Task HandleAsync(UserCreatedEvent @event)
        {
            string emailVerificationLink = $@"/api/users/verifyEmail?code={@event.VerificationCode}";

            _stringBuilder.Clear();
            _stringBuilder.AppendLine($"Hi {@event.UserName},");
            _stringBuilder.AppendLine();
            _stringBuilder.AppendLine();
            _stringBuilder.AppendLine("In order to activate your account please verify your email address by clicking the link below: ");
            _stringBuilder.AppendLine(emailVerificationLink);
            _stringBuilder.AppendLine();
            _stringBuilder.AppendLine("Thanks,");
            _stringBuilder.AppendLine("Microservices Demo");

            await this._emailHandler.SendMailAsync("Microservices Demo Email Verification", _stringBuilder.ToString(), @event.Email);

            await this._hubContext.Clients.Client(@event.ConnectionId).SendAsync("GetCreateUserResponse", new { status = StatusCodes.Status200OK });
        }
    }
}
