using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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
            localizationOptions.SupportedCultures = configuration.GetSection("localization:SupportedCultures").Get<List<string>>();

            List<CultureInfo> supportedCultures = new List<CultureInfo>();
            if (!localizationOptions.SupportedCultures.Any(culture => culture == "en-US"))
            {
                localizationOptions.SupportedCultures.Add("en-US");
            }

            foreach (string supportedCulture in localizationOptions.SupportedCultures)
            {
                supportedCultures.Add(new CultureInfo(supportedCulture));
            }

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
            });

            services.AddSingleton(serviceProvider =>
            {
                IStringLocalizerFactory localizerFactory = serviceProvider.GetService<IStringLocalizerFactory>();
                return new LocalizationService(localizerFactory, typeof(T));
            });
        }

        public static void UseLocalizationService(this IApplicationBuilder applicationBuilder)
        {
            IOptions<RequestLocalizationOptions> requestLocalizationOptions = applicationBuilder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            applicationBuilder.UseRequestLocalization(requestLocalizationOptions.Value);
        }
    }
}
