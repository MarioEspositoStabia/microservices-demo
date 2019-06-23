using MongoDB.Driver;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories.NoSQL
{
    public class MongoRepository<TModel> : IMongoDBRepository<TModel> where TModel : class
    {
        private readonly IMongoDatabase _database;
        protected IMongoCollection<TModel> Collection;

        public MongoRepository(IMongoDatabase database)
        {
            this._database = database;
            this.Collection = this._database.GetCollection<TModel>(typeof(TModel).Name);
        }

        public async Task AddAsync(TModel model)
        {
            await this.Collection.InsertOneAsync(model);
        }

        public async Task DeleteAsync(FilterDefinition<TModel> filter)
        {
            await this.Collection.DeleteOneAsync(filter);
        }

        public async Task<IAsyncCursor<TModel>> GetAllAsync()
        {
            return await this.Collection.FindAsync(model => true);
        }

        public async Task<IAsyncCursor<TModel>> GetByAsync(FilterDefinition<TModel> filter)
        {
            return await this.Collection.FindAsync(filter);
        }

        public async Task UpdateAsync(FilterDefinition<TModel> filter, UpdateDefinition<TModel> update)
        {
             await this.Collection.UpdateOneAsync(filter, update);
        }
    }
}
