using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shared.Application.Common.Interfaces.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    }
}
