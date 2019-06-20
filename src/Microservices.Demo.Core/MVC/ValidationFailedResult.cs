using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microservices.Demo.Core.MVC
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState, string message)
            : base(new ValidationResultModel(modelState, message))
        {
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
