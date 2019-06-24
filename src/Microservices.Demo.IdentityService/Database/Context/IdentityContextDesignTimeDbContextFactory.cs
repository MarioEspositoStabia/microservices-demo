using Microservices.Demo.Core.Enumerations;
using Microsoft.EntityFrameworkCore.Design;

namespace Microservices.Demo.IdentityService.Database.Context
{
    public class IdentityContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            return new IdentityContext(
                DatabaseProvider.MicrosoftSQLServer,
                $@"Server=127.0.0.1,1433\Enterprise;Integrated Security=False;User Id=sa;Password=Ankara.06;MultipleActiveResultSets=True;Initial Catalog=Microservices.Demo.Identity;");
        }
    }
}
