using Microservices.Demo.Core.Commands;

namespace Microservices.Demo.IdentityService.Messaging.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string ConnectionId { get; set; }
    }
}
