using Microservices.Demo.Core.Commands;

namespace Microservices.Demo.ProductService.Messaging.Commands
{
    public class GetProductsCommand : ICommand
    {
        public string Token { get; set; }
        public string ConnectionId { get; set; }
    }
}
