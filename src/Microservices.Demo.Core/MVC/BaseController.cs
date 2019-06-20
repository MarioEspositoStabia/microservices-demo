using Microservices.Demo.Core.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microservices.Demo.Core.MVC
{
    public abstract class BaseController : Controller
    {
        public const string URLHELPER = "URLHELPER";

        public BaseController(LocalizationService localizationService)
        {
            this.LocalizationService = localizationService;
        }

        protected LocalizationService LocalizationService { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[URLHELPER] = this.Url;
        }

        public new OkObjectResult Ok()
        {
            return base.Ok(new { status = this.LocalizationService.GetLocalizedStatusCode(ResponseCode.Success) });
        }

        public override OkObjectResult Ok(object value)
        {
            return base.Ok(new { response = value, status = this.LocalizationService.GetLocalizedStatusCode(ResponseCode.Success) });
        }

        public OkObjectResult Ok(object value, StatusCode statusCode)
        {
            return base.Ok(new { response = value, status = statusCode });
        }
    }
}
