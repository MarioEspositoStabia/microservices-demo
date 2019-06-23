using Microservices.Demo.Core.Repositories.NoSQL;
using Microservices.Demo.ProductService.Database.Documents;
using MongoDB.Driver;

namespace Microservices.Demo.ProductService.Repositories
{
    public class ProductRepository : MongoRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
