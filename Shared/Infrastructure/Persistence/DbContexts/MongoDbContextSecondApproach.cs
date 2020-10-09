using System;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Settings;
using Shared.Application.Common.Interfaces.DbContext;

namespace Shared.Infrastructure.Persistence.DbContexts
{
    public sealed class MongoDbContextSecondApproach : IMongoDbContextSecondApproach
    {
        IMongoClient MongoClient { get; set; }
        IMongoDatabase MongoDatabase { get; set; }

        public MongoDbContextSecondApproach(IOptions<MongoDBSettings> mongoDbSettings)
        {
            if (MongoClient is object)
                return;

            MongoClient = new MongoClient(mongoDbSettings.Value.Connection);
            MongoDatabase = MongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return MongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
        {
            return MongoDatabase.GetCollection<TEntity>(name);
        }

        public async Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken)
        {
            return await MongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
