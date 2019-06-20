namespace Microservices.Demo.Core.Events
{
    public interface IRejectedEvent : IEvent
    {
        string Code { get; }
        string Description { get; }
    }
}
