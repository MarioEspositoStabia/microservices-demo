using Microservices.Demo.ProductService.Database.Documents;
using Microservices.Demo.ProductService.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Microservices.Demo.ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public async Task<IAsyncCursor<Product>> GatAllProductsAsync()
        {
            return await this._productRepository.GetAllAsync();
        }
    }
}
