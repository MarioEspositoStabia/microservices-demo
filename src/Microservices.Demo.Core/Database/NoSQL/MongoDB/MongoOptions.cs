namespace Microservices.Demo.Core.Database.NoSQL.MongoDB
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool Seed { get; set; }
    }
}
