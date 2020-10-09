using System;
using System.Threading;
using Shared.Domain.Entities;
using System.Threading.Tasks;
using Shared.Application.Common.Interfaces.Services;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.Repository;

namespace Shared.Application.Services
{
    public class MyServiceFirstApproach : IMyService
    {
        public MyServiceFirstApproach(IMongoDbContextFirstApproach mongoDbContext, IGenericRepository<Product> productRepository)
        {
            _mongoDbContext = mongoDbContext;
            _productRepository = productRepository;
        }

        readonly IMongoDbContextFirstApproach _mongoDbContext;
        readonly IGenericRepository<Product> _productRepository;

        public async Task<string> InsertOnMongoDBAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _mongoDbContext.BeginTransactionAsync(cancellationToken);

                var tv = new Product { Description = "Television", SKU = 4001, Price = 2000 };
                var book = new Product { Description = "A funny book", SKU = 43221, Price = 19.99 };
                var dogBowl = new Product { Description = "Bowl for Fido", SKU = 123, Price = 40.00 };

                await _productRepository.InsertAsync(tv, cancellationToken);
                await _productRepository.InsertAsync(book, cancellationToken);
                await _productRepository.InsertAsync(dogBowl, cancellationToken);

                await _mongoDbContext.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return $"'{nameof(InsertOnMongoDBAsync)}()' -> [Exception: {ex.Message}]";
            }

            return $"{nameof(InsertOnMongoDBAsync)}() -> Done!";
        }
    }
}
