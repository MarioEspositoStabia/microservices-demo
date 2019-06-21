using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class CreateUserRejectedEvent : IRejectedEvent
    {
        public CreateUserRejectedEvent(string email,
            ApiError apiError, string code)
        {
            this.Email = email;
            this.ApiError = apiError;
            this.Code = code;
        }

        public CreateUserRejectedEvent(string email,
            string message, string code)
        {
            this.Email = email;
            this.Message = message;
            this.Code = code;
        }

        public string Email { get; }

        public ApiError ApiError { get; }

        public string Message { get; }

        public string Code { get; }

        public string ConnectionId { get; set; }

    }
}
