using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Microservices.Demo.Core.MVC
{
    public class ApiError
    {
        public ApiError(StatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public ApiError(string message)
        {
            this.Message = message;
        }

        public ApiError(string message, string detail)
            : this(message)
        {
            this.Detail = detail;
        }

        public ApiError(StatusCode statusCode, string message)
            : this(statusCode)
        {
            this.Message = message;
        }

        public ApiError(StatusCode statusCode, string message, string detail)
            : this(statusCode, message)
        {
            this.Detail = detail;
        }
        
        public ApiError(StatusCode statusCode, ModelStateDictionary modelState, string message)
            : this(statusCode, message)
        {
            this.ValidationResult = new ValidationResultModel(modelState, message);
            this.Message = message;
        }

        public ApiError(StatusCode statusCode, ModelStateDictionary modelState, string message, string detail)
            : this(statusCode, modelState, message)
        {
            this.Detail = detail;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public StatusCode StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ValidationResultModel ValidationResult { get; set; }
    }
}
