using Microservices.Demo.Core.Events;

namespace Microservices.Demo.ProductService.Messaging.Events
{
    public class GetProductsEvent : IEvent
    {
        public GetProductsEvent(string jsonProducts)
        {
            this.JsonProducts = jsonProducts;
        }

        public string JsonProducts { get; set; }

        public string ConnectionId { get; set; }
    }
}
