using Microservices.Demo.Core.Commands;

namespace Microservices.Demo.IdentityService.Messaging.Commands
{
    public class RefreshTokenCommand : ICommand
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string ConnectionId { get; set; }
    }
}
