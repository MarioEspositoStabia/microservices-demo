using System.Threading.Tasks;

namespace Microservices.Demo.Core.Events
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
