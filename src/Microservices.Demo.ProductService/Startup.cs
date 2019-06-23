using Microservices.Demo.Core.Database.NoSQL.MongoDB;
using Microservices.Demo.Core.Database.Relational;
using Microservices.Demo.Core.MVC;
using Microservices.Demo.Core.MVC.Authentication;
using Microservices.Demo.Core.RabbitMq;
using Microservices.Demo.Core.Redis;
using Microservices.Demo.ProductService.Database;
using Microservices.Demo.ProductService.Messaging.Commands;
using Microservices.Demo.ProductService.Repositories;
using Microservices.Demo.ProductService.Resources;
using Microservices.Demo.ProductService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.ProductService
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
            services.AddJwt(Configuration);
            services.AddRedis(Configuration);
            services.AddLocalizationService<SharedResource>(Configuration);

            services.AddMongoDB<MongoSeeder>(Configuration);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, Services.ProductService>();

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

            app.RabbitMqSubscribeToCommand<GetProductsCommand>();
            app.UseLocalizationService();
            app.InitializeDatabase();
        }
    }
}
