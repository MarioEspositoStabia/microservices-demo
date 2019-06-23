using Microservices.Demo.Core.Commands;
namespace Microservices.Demo.IdentityService.Messaging.Commands
{
    public class GetTokenCommand : ICommand
    {
        public string UserNameOrEmail { get; set; }

        public string Password { get; set; }

        public string Ip { get; set; }

        public string ConnectionId { get; set; }
    }
}
