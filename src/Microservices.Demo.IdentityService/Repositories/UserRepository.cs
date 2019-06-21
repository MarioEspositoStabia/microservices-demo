using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Repositories;
using Microservices.Demo.IdentityService.Database.Entity;

namespace Microservices.Demo.IdentityService.Repositories
{
    public class UserRepository : AuditableRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {

        }
    }
}
