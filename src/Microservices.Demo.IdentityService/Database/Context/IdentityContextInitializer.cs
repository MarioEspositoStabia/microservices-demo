using Microservices.Demo.Core.Database;
using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Enumerations;
using Microservices.Demo.IdentityService.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Demo.IdentityService.Database.Context
{
    public class IdentityContextInitializer : IDatabaseInitializer
    {
        private readonly IDbContext _context;

        public IdentityContextInitializer(IDbContext context)
        {
            this._context = context;
        }

        public Task InitializeAsync()
        {
            IdentityContext identityContext = this._context as IdentityContext;

            if (identityContext != null)
            {
                if (!identityContext.Users.Any())
                {
                    Guid adminId = Guid.NewGuid();

                    User user = new User("TestUser", "test.1234", "test@microservices.demo")
                    {
                        EmailConfirmed = true,
                        Id = adminId,
                        Status = EntityStatus.Available
                    };

                    identityContext.Users.Add(user);

                    identityContext.SaveChanges(Guid.Empty);
                }
            }

            return Task.CompletedTask;
        }
    }
}
