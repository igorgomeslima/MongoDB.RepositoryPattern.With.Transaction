using Shared.Domain.Entities;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.Repository;

namespace Shared.Infrastructure.Persistence.Repository.MongoDB._Approachs.First
{
    public class ProductRepositoryMongoDBFirstApproach : GenericRepositoryMongoDBFirstApproach<Product>, IProductRepository
    {
        public ProductRepositoryMongoDBFirstApproach(IMongoDbContextFirstApproach mongoDbContext): base(mongoDbContext)
        {
            //Put here SPECIFIC methods of ProductRepository...
        }
    }
}
