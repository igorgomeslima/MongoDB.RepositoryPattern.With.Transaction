using System;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Settings;
using Shared.Application.Common.Interfaces.DbContext;

namespace Shared.Infrastructure.Persistence.DbContexts
{
    public sealed class MongoDbContextFirstApproach : IMongoDbContextFirstApproach
    {
        IMongoClient MongoClient { get; set; }
        IClientSessionHandle Session { get; set; }
        IMongoDatabase MongoDatabase { get; set; }

        public MongoDbContextFirstApproach(IOptions<MongoDBSettings> mongoDbSettings)
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

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            Session.StartTransaction();
        }

        public async Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken)
        {
            return await MongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await Session.CommitTransactionAsync(cancellationToken);
        }

        public IClientSessionHandle GetCurrentTransactionSession()
        {
            return Session;
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
