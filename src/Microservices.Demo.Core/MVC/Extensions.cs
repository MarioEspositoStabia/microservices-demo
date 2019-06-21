using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
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

        public static void AddLocalizationService<T>(this IServiceCollection services, List<CultureInfo> supportedCultures) where T : class
        {
            if (!supportedCultures.Any(culture => culture.Name == "en-US"))
            {
                supportedCultures.Add(new CultureInfo("en-US"));
            }

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
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
