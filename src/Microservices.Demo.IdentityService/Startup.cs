using Microservices.Demo.Core.Database;
using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.Enumerations;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.Core.RabbitMq;
using Microservices.Demo.IdentityService.Database.Context;
using Microservices.Demo.IdentityService.Messaging.Commands;
using Microservices.Demo.IdentityService.Repositories;
using Microservices.Demo.IdentityService.Resources;
using Microservices.Demo.IdentityService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace Microservices.Demo.IdentityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddRabbitMq(Configuration);

            services.AddScoped<IDbContext, IdentityContext>(serviceProvider => new IdentityContext(DatabaseProvider.MicrosoftSQLServer, Configuration.GetSection("Data:ConnectionString").Value));
            services.AddScoped<IDatabaseInitializer, IdentityContextInitializer>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddLocalizationService<SharedResource>(new List<CultureInfo> { new CultureInfo("en-US") });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IDatabaseInitializer>().InitializeAsync();
            }

            app.UseLocalizationService();

            app.SubscribeToCommand<CreateUserCommand>();
        }
    }
}
