using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace Microservices.Demo.Core.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public ValidateModelAttribute()
        {
            this.Order = 1;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
            {
                LocalizationService localizationService = context.HttpContext.RequestServices.GetService<LocalizationService>();
                CultureInfo culture = context.HttpContext.GetRequestCulture();

                context.Result = new ValidationFailedResult(context.ModelState, localizationService.GetLocalizedHtmlString(context.ModelState.ValidationState.ToString()));
            }
        }
    }
}
