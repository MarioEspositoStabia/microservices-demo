using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class RefreshTokenRejectedEvent : IRejectedEvent
    {
        public RefreshTokenRejectedEvent(string message, string code)
        {
            this.Message = message;
            this.Code = code;
        }

        public string Code { get; set; }

        public ApiError ApiError { get; set; }

        public string Message { get; set; }

        public string ConnectionId { get; set; }
    }
}
