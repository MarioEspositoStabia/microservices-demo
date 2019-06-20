using System.Threading.Tasks;

namespace Microservices.Demo.Core.Database
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
