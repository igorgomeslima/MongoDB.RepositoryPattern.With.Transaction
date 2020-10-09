using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces.DbContext
{
    public interface IMongoDbContextFirstApproach : IMongoDbContext
    {
        //Put here SPECIFIC methods of first approach...
        IClientSessionHandle GetCurrentTransactionSession();
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}