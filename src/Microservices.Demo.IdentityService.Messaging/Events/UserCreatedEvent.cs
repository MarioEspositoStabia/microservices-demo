using Microservices.Demo.Core.Events;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class UserCreatedEvent : IEvent
    {
        public UserCreatedEvent(string userName, string email, string verificationCode)
        {
            this.UserName = userName;
            this.Email = email;
            this.VerificationCode = verificationCode;
        }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string VerificationCode { get; set; }

        public string ConnectionId { get; set; }
    }
}
