using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.Core.Events
{
    public interface IRejectedEvent : IEvent
    {
        string Code { get; }
        ApiError ApiError { get; }
        string Message { get; }
    }
}
