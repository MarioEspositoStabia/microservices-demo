using Microservices.Demo.Core.Repositories.NoSQL;
using Microservices.Demo.ProductService.Database.Documents;

namespace Microservices.Demo.ProductService.Repositories
{
    public interface IProductRepository : IMongoDBRepository<Product>
    {
    }
}
