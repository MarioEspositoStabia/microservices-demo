using Microservices.Demo.Core.Events;

namespace Microservices.Demo.IdentityService.Messaging.Events
{
    public class UserCreatedEvent : IEvent
    {
        public UserCreatedEvent(string email, string verificationCode)
        {
            this.Email = email;
            this.VerificationCode = verificationCode;
        }

        public string Email { get; set; }

        public string VerificationCode { get; set; }

        public string ConnectionId { get; set; }
    }
}
