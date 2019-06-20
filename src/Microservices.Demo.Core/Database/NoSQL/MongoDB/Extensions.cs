using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Microservices.Demo.Core.Database.NoSQL.MongoDB
{
    public static class Extensions
    {
        public static void AddMongoDB<TMongoSeeder>(this IServiceCollection services, IConfiguration configuration) where TMongoSeeder : class, IDatabaseSeeder
        {
            IConfigurationSection mongoConfigurationSection = configuration.GetSection("mongo");
            services.Configure<MongoOptions>(mongoConfigurationSection);
            services.AddSingleton(serviceProvider =>
            {
                IOptions<MongoOptions> options = serviceProvider.GetService<IOptions<MongoOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });

            services.AddScoped(serviceProvider =>
            {
                IOptions<MongoOptions> options = serviceProvider.GetService<IOptions<MongoOptions>>();
                MongoClient client = serviceProvider.GetService<MongoClient>();

                return client.GetDatabase(options.Value.Database);
            });

            services.AddScoped<IDatabaseSeeder, TMongoSeeder>();

            services.AddScoped<IDatabaseInitializer>(serviceProvider =>
            {
                IOptions<MongoOptions> options = serviceProvider.GetService<IOptions<MongoOptions>>();
                IMongoDatabase database = serviceProvider.GetService<IMongoDatabase>();
                IDatabaseSeeder seeder = serviceProvider.GetService<IDatabaseSeeder>();
                return new MongoInitializer(database, seeder, options);
            });
        }
    }
}
