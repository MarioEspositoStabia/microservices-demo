using Microservices.Demo.ProductService.Database.Documents;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Microservices.Demo.ProductService.Services
{
    public interface IProductService
    {
        Task<IAsyncCursor<Product>> GatAllProductsAsync();
    }
}
