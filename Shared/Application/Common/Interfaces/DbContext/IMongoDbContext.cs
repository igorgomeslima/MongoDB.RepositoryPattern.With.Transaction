using MongoDB.Driver;

namespace Shared.Application.Common.Interfaces.DbContext
{
    public interface IMongoDbContext
    {
        IMongoCollection<TEntity> GetCollection<TEntity>();
        IMongoCollection<TEntity> GetCollection<TEntity>(string name);
    }
}