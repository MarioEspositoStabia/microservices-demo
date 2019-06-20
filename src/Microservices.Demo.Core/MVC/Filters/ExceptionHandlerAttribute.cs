using Microservices.Demo.Core.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;

namespace Microservices.Demo.Core.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            LocalizationService localizationService = context.HttpContext.RequestServices.GetService<LocalizationService>();
            CultureInfo culture = context.HttpContext.GetRequestCulture();
            ApiError apiError = null;

            if (context.Exception is SecurityTokenExpiredException)
            {
                SecurityTokenExpiredException securityTokenExpiredException = context.Exception as SecurityTokenExpiredException;

                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                apiError = new ApiError(localizationService.GetLocalizedStatusCode(ResponseCode.SecurityTokenExpired));
            }
            else if (context.Exception is SecurityTokenException)
            {
                apiError = new ApiError(localizationService.GetLocalizedStatusCode(ResponseCode.InvalidSecurityToken));

                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError(localizationService.GetLocalizedStatusCode(ResponseCode.UnauthorizedAccess));

                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                string message = string.Empty;
                string stack = null;
#if DEBUG
                message = context.Exception.GetBaseException().Message;
                stack = context.Exception.StackTrace;
#endif

                apiError = new ApiError(message, stack);

                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
