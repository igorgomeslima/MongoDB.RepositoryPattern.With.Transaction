using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.Repository;
using Shared.Application.Common.Interfaces.UnitOfWork;

namespace Shared.Infrastructure.Persistence.Repository.MongoDB._Approachs.Second
{
    public class GenericRepositoryMongoDBSecondApproach<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepositoryMongoDBSecondApproach(IMongoDbContextSecondApproach mongoDbContext, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mongoCollection = mongoDbContext.GetCollection<TEntity>();
        }

        readonly IUnitOfWork _unitOfWork;
        readonly IMongoCollection<TEntity> _mongoCollection;

        public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var transaction = _unitOfWork.GetCurrentTransactionSession<IClientSessionHandle>();

            switch (transaction)
            {
                case null:
                    await _mongoCollection.InsertOneAsync(entity, cancellationToken: cancellationToken);
                    break;
                default:
                    await _mongoCollection.InsertOneAsync(transaction, entity, cancellationToken: cancellationToken);
                    break;
            }
        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            var transaction = _unitOfWork.GetCurrentTransactionSession<IClientSessionHandle>();

            switch (transaction)
            {
                case null:
                    await _mongoCollection.InsertManyAsync(entities, cancellationToken: cancellationToken);
                    break;
                default:
                    await _mongoCollection.InsertManyAsync(transaction, entities, cancellationToken: cancellationToken);
                    break;
            }
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
