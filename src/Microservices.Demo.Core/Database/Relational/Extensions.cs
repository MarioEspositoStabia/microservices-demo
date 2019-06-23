using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microservices.Demo.Core.Database.Relational
{
    public static class Extensions
    {
        public static void AddDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration) where TDbContext : BaseContext
        {
            DbContextOptions dbContextOptions = new DbContextOptions();
            IConfigurationSection configurationSection = configuration.GetSection("relationalDatabase");
            configurationSection.Bind(dbContextOptions);

            services.AddScoped<IDbContext, TDbContext>(serviceProvider => (TDbContext)Activator.CreateInstance(typeof(TDbContext), dbContextOptions.DatabaseProvider, dbContextOptions.ConnectionString));
        }

        public static void AddDbContext<TDbContext, TDatabaseInitializer>(this IServiceCollection services, IConfiguration configuration) where TDbContext : BaseContext where TDatabaseInitializer : class, IDatabaseInitializer
        {
            DbContextOptions dbContextOptions = new DbContextOptions();
            IConfigurationSection configurationSection = configuration.GetSection("relationalDatabase");
            configurationSection.Bind(dbContextOptions);

            services.AddScoped<IDbContext, TDbContext>(serviceProvider => (TDbContext)Activator.CreateInstance(typeof(TDbContext), dbContextOptions.DatabaseProvider, dbContextOptions.ConnectionString));

            services.AddScoped<IDatabaseInitializer, TDatabaseInitializer>();
        }

        public static void InitializeDatabase(this IApplicationBuilder applicationBuilder)
        {
            using (IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                IDatabaseInitializer databaseInitializer = serviceScope.ServiceProvider.GetService<IDatabaseInitializer>();

                if (databaseInitializer != null)
                {
                    serviceScope.ServiceProvider.GetService<IDatabaseInitializer>().InitializeAsync();
                }
            }
        }
    }
}
