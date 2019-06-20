using System.Threading.Tasks;

namespace Microservices.Demo.Core.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
