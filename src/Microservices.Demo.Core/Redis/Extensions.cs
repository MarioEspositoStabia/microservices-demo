using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Microservices.Demo.Core.Redis
{
    public static class Extensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            RedisOptions redisOptions = new RedisOptions();
            IConfigurationSection configurationSection = configuration.GetSection("redis");
            configurationSection.Bind(redisOptions);

            services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(redisOptions.Configuration));
        }
    }
}
