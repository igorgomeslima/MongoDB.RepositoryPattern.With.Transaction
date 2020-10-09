using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces.DbContext
{
    public interface IMongoDbContextSecondApproach : IMongoDbContext
    {
        //Put here SPECIFIC methods of second approach...
        Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken);
    }
}