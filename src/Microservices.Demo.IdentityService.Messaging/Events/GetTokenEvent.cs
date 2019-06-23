using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class GetTokenEvent : IEvent
    {
        public GetTokenEvent(RefreshTokenModel token)
        {
            this.Token = Token;
        }

        public RefreshTokenModel Token { get; set; }

        public string ConnectionId { get; set; }
    }
}
