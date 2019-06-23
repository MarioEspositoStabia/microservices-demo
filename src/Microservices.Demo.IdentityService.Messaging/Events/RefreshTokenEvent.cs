using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class RefreshTokenEvent : IEvent
    {
        public RefreshTokenEvent(RefreshTokenModel refreshToken)
        {
            this.Token = refreshToken;
        }

        public RefreshTokenModel Token { get; set; }

        public string ConnectionId { get; set; }
    }
}
