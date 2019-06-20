using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Database.NoSQL.MongoDB
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly bool _seed;
        private readonly IMongoDatabase _database;
        private readonly IDatabaseSeeder _seeder;

        public MongoInitializer(IMongoDatabase database,
            IDatabaseSeeder seeder,
            IOptions<MongoOptions> options)
        {
            this._database = database;
            this._seeder = seeder;
            this._seed = options.Value.Seed;
        }

        public async Task InitializeAsync()
        {
            if (this._initialized)
            {
                return;
            }

            RegisterConventions();

            this._initialized = true;

            if (!this._seed)
            {
                return;
            }

            await this._seeder.SeedAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("MongoDBConventions", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}
