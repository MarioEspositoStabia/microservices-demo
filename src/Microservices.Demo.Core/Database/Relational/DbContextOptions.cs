using Microservices.Demo.Core.Enumerations;

namespace Microservices.Demo.Core.Database.Relational
{
    public class DbContextOptions
    {
        public string ConnectionString { get; set; }

        public DatabaseProvider DatabaseProvider { get; set; }

        public bool LogAuditableTables { get; set; }
    }
}
