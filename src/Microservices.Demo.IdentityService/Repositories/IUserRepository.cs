using Microservices.Demo.Core.Repositories;
using Microservices.Demo.IdentityService.Database.Entity;

namespace Microservices.Demo.IdentityService.Repositories
{
    public interface IUserRepository : IAuditableRepository<User>
    {
    }
}
