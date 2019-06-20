using System.Threading.Tasks;

namespace Microservices.Demo.Core.Database
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
