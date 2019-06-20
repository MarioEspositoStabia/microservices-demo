using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Demo.Core.MVC
{
    public class ValidationResultModel
    {
        public ValidationResultModel(ModelStateDictionary modelState, string message)
        {
            this.Message = message;
            this.Errors = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage))).ToList();
        }

        public string Message { get; }

        public List<ValidationError> Errors { get; }
    }
}
