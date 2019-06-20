using Microservices.Demo.Core.Enumerations;
using Newtonsoft.Json;
using System;

namespace Microservices.Demo.Core.MVC
{
    public class StatusCode
    {
        public string Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeveloperMessage { get; set; }

        public string Message { get; set; }

        public StatusType Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri MoreInfo { get; set; }
    }
}
