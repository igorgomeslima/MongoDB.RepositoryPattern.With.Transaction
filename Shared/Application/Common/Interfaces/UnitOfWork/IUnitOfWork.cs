using System.Threading;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        TTransactionSessionType GetCurrentTransactionSession<TTransactionSessionType>();

        Task CommitAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
