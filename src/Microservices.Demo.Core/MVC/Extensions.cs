using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq;

namespace Microservices.Demo.Core.MVC
{
    public static class Extensions
    {
        public static CultureInfo GetRequestCulture(this HttpContext context)
        {
            IRequestCultureFeature requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
            return requestCultureFeature.RequestCulture.Culture;
        }

        public static void AddLocalizationService<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            LocalizationOptions localizationOptions = new LocalizationOptions();
            IConfigurationSection configurationSection = configuration.GetSection("localization");
            configurationSection.Bind(localizationOptions);

            if (!localizationOptions.SupportedCultures.Any(culture => culture.Name == "en-US"))
            {
                localizationOptions.SupportedCultures.Add(new CultureInfo("en-US"));
            }

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = localizationOptions.SupportedCultures;
                options.SupportedUICultures = localizationOptions.SupportedCultures;
                options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
            });

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IStringLocalizerFactory localizerFactory = serviceProvider.GetService<IStringLocalizerFactory>();
            services.AddSingleton(x => new LocalizationService(localizerFactory, typeof(T)));
        }

        public static void UseLocalizationService(this IApplicationBuilder applicationBuilder)
        {
            IOptions<RequestLocalizationOptions> requestLocalizationOptions = applicationBuilder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            applicationBuilder.UseRequestLocalization(requestLocalizationOptions.Value);
        }
    }
}
