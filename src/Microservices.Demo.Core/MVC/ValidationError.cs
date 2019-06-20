using Newtonsoft.Json;

namespace Microservices.Demo.Core.MVC
{
    public class ValidationError
    {
        public ValidationError(string field, string message)
        {
            this.Field = string.IsNullOrEmpty(field) ? null : field;
            this.Message = message;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }
    }
}
