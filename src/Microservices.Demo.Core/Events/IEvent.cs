namespace Microservices.Demo.Core.Events
{
    public interface IEvent
    {
        string ConnectionId { get; }
    }
}
