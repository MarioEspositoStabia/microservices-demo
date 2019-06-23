using Microservices.Demo.Core.Commands;
using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.Core.MVC.Utils;
using Microservices.Demo.Core.RabbitMq;
using Microservices.Demo.Core.Redis;
using Microservices.Demo.IdentityService.Database.Context;
using Microservices.Demo.IdentityService.Handlers;
using Microservices.Demo.IdentityService.Messaging.Commands;
using Microservices.Demo.IdentityService.Repositories;
using Microservices.Demo.IdentityService.Resources;
using Microservices.Demo.IdentityService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddLocalizationService<SharedResource>(Configuration);
            services.AddDbContext<IdentityContext, IdentityContextInitializer>(Configuration);
            services.AddRedis(Configuration);
            services.AddEmailHandler(Configuration);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<ICommandHandler<GetTokenCommand>, GetTokenCommandHandler>();
            services.AddScoped<ICommandHandler<RefreshTokenCommand>, RefreshTokenCommandHandler>();

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

            app.UseLocalizationService();
            app.RabbitMqSubscribeToCommand<CreateUserCommand>();
            app.RabbitMqSubscribeToCommand<GetTokenCommand>();
            app.RabbitMqSubscribeToCommand<RefreshTokenCommand>();
            app.InitializeDatabase();
        }
    }
}
