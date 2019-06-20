using MongoDB.Driver;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.Repositories.NoSQL
{
    public interface IMongoDBRepository<TModel> where TModel : class
    {
        Task<IAsyncCursor<TModel>> GetAllAsync();

        Task<IAsyncCursor<TModel>> GetByAsync(FilterDefinition<TModel> filter);

        Task AddAsync(TModel model);

        Task UpdateAsync(FilterDefinition<TModel> filter, UpdateDefinition<TModel> update);

        Task DeleteAsync(FilterDefinition<TModel> filter);
    }
}
