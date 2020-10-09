using System.Threading;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces.Services
{
    public interface IMyService
    {
        Task<string> InsertOnMongoDBAsync(CancellationToken cancellationToken = default);
    }
}
