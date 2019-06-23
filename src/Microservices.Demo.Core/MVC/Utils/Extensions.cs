using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Demo.Core.MVC.Utils
{
    public static class Extensions
    {
        public static void AddEmailHandler(this IServiceCollection services, IConfiguration configuration)
        {
            EmailOptions emailOptions = new EmailOptions();
            IConfigurationSection section = configuration.GetSection("email");
            section.Bind(emailOptions);

            services.AddSingleton<IEmailHandler>(serviceProvider => new EmailHandler(emailOptions));
        }
    }
}
