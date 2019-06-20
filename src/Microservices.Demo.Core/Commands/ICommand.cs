namespace Microservices.Demo.Core.Commands
{
    public interface ICommand
    {
        string ConnectionId { get; }
    }
}
