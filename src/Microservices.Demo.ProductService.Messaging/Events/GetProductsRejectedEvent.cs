using Microservices.Demo.Core.Events;
using Microservices.Demo.Core.MVC;

namespace Microservices.Demo.ProductService.Messaging.Events
{
    public class GetProductsRejectedEvent : IRejectedEvent
    {
        public GetProductsRejectedEvent(string message, string code)
        {
            this.Message = message;
            this.Code = code;
        }

        public string Code { get; set; }

        public ApiError ApiError { get; set; }

        public string Message { get; set; }

        public string ConnectionId { get; set; }
    }
}
