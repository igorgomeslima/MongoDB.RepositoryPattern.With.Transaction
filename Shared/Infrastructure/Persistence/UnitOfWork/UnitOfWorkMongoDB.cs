using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.UnitOfWork;

namespace Shared.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWorkMongoDB : IUnitOfWork
    {
        IClientSessionHandle _session;
        readonly IMongoDbContextSecondApproach _mongoDbContext;

        public UnitOfWorkMongoDB(IMongoDbContextSecondApproach mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _session.CommitTransactionAsync();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            _session = await _mongoDbContext.StartSessionAsync(cancellationToken);
            _session.StartTransaction();
        }

        public TTransactionSessionType GetCurrentTransactionSession<TTransactionSessionType>() => (TTransactionSessionType)_session;
    }
}
