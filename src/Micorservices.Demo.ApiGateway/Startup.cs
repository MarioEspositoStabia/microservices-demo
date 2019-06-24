using Micorservices.Demo.ApiGateway.Handlers;
using Micorservices.Demo.ApiGateway.Resources;
using Microservices.Demo.Core.Elasticsearch;
using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.Core.MVC.Authentication;
using Microservices.Demo.Core.MVC.Utils;
using Microservices.Demo.Core.RabbitMq;
using Microservices.Demo.IdentityService.Messaging.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Micorservices.Demo.ApiGateway
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

            services.AddElasticsearch(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddEmailHandler(Configuration);
            services.AddJwt(Configuration);
            services.AddLocalizationService<SharedResource>(Configuration);
            services.AddSignalR();

            services.AddScoped<IEventHandler<UserCreatedEvent>, UserCreatedEventHandler>();
            services.AddScoped<IEventHandler<CreateUserRejectedEvent>, CreateUserRejectedEventHandler>();
            services.AddScoped<IEventHandler<GetTokenEvent>, GetTokenEventHandler>();
            services.AddScoped<IEventHandler<GetTokenRejectedEvent>, GetTokenRejectedEventHandler>();
            services.AddScoped<IEventHandler<RefreshTokenEvent>, RefreshTokenEventHandler>();
            services.AddScoped<IEventHandler<RefreshTokenRejectedEvent>, RefreshTokenRejectedEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddSerilog();

            app.UseSignalR(configuration =>
            {
                configuration.MapHub<ApiGatewayHub>("/apiGateWay");
            });

            app.UseLocalizationService();

            app.RabbitMqSubscribeToEvent<UserCreatedEvent>();
            app.RabbitMqSubscribeToEvent<CreateUserRejectedEvent>();
            app.RabbitMqSubscribeToEvent<GetTokenEvent>();
            app.RabbitMqSubscribeToEvent<GetTokenRejectedEvent>();
            app.RabbitMqSubscribeToEvent<RefreshTokenEvent>();
            app.RabbitMqSubscribeToEvent<RefreshTokenRejectedEvent>();
        }
    }
}
