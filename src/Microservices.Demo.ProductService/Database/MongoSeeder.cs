using Microservices.Demo.Core.Database;
using Microservices.Demo.ProductService.Database.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Demo.ProductService.Database
{
    public class MongoSeeder : IDatabaseSeeder
    {
        protected readonly IMongoDatabase Database;

        public MongoSeeder(IMongoDatabase database)
        {
            this.Database = database;
        }

        public async Task SeedAsync()
        {
            IAsyncCursor<BsonDocument> collectionsCursor = await this.Database.ListCollectionsAsync();
            List<BsonDocument> collections = await collectionsCursor.ToListAsync();

            if (collections.Any())
            {
                return;
            }

            IMongoCollection<Product> products = this.Database.GetCollection<Product>(typeof(Product).Name);

            Product product1 = new Product("P1", "Product 1", "Product 1 description");
            product1.Id = Guid.NewGuid();
            await products.InsertOneAsync(product1);

            Product product2 = new Product("P2", "Product 2", "Product 2 description");
            product2.Id = Guid.NewGuid();
            await products.InsertOneAsync(product2);
        }
    }
}
