using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Microservices.Demo.Core.MVC
{
    public static class Extensions
    {
        public static CultureInfo GetRequestCulture(this HttpContext context)
        {
            IRequestCultureFeature requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
            return requestCultureFeature.RequestCulture.Culture;
        }
    }
}
