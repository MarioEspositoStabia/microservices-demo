using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class GetTokenRejectedEvent : IRejectedEvent
    {
        public GetTokenRejectedEvent(string userNameOrEmail,
            ApiError apiError, string code)
        {
            this.UserNameOrEmail = userNameOrEmail;
            this.ApiError = apiError;
            this.Code = code;
        }

        public GetTokenRejectedEvent(string userNameOrEmail, string message, string code)
        {
            this.UserNameOrEmail = userNameOrEmail;
            this.Message = message;
            this.Code = code;
        }

        public string UserNameOrEmail { get; set; }

        public string Code { get; set; }

        public ApiError ApiError { get; set; }

        public string Message { get; set; }

        public string ConnectionId { get; set; }
    }
}
