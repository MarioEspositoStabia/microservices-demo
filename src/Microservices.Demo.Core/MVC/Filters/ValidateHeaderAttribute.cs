using Microservices.Demo.Core.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;
using System.Linq;


namespace Microservices.Demo.Core.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateHeaderAttribute : Attribute, IResourceFilter
    {
        private const string GrantType = "GRANT-TYPE";

        private readonly string[] _grantTypes = { "token", "refresh-token" };

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.Keys.Contains(GrantType))
            {
                StringValues grantType = context.HttpContext.Request.Headers[GrantType];

                if (!this._grantTypes.Any(gt => gt == grantType))
                {
                    LocalizationService localizationService = context.HttpContext.RequestServices.GetService<LocalizationService>();
                    CultureInfo culture = context.HttpContext.GetRequestCulture();

                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(new ApiError(localizationService.GetLocalizedStatusCode(ResponseCode.MissingHeader)));
                }
            }
        }
    }
}
