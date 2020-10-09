using Shared.Domain.Entities;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.Repository;
using Shared.Application.Common.Interfaces.UnitOfWork;

namespace Shared.Infrastructure.Persistence.Repository.MongoDB._Approachs.Second
{
    public class ProductRepositoryMongoDBSecondApproach : GenericRepositoryMongoDBSecondApproach<Product>, IProductRepository
    {
        public ProductRepositoryMongoDBSecondApproach(IMongoDbContextSecondApproach mongoDbContext, IUnitOfWork unitOfWork) 
            : base(mongoDbContext, unitOfWork)
        {
            //Put here SPECIFIC methods of ProductRepository...
        }
    }
}
