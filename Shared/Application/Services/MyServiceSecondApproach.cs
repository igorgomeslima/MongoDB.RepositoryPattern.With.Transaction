using System;
using System.Threading;
using Shared.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.Application.Common.Interfaces.Services;
using Shared.Application.Common.Interfaces.Repository;
using Shared.Application.Common.Interfaces.UnitOfWork;

namespace Shared.Application.Services
{
    public class MyServiceSecondApproach : IMyService
    {
        public MyServiceSecondApproach(IUnitOfWork unitOfWork, IGenericRepository<Product> productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        readonly IUnitOfWork _unitOfWork;
        readonly IGenericRepository<Product> _productRepository;

        public async Task<string> InsertOnMongoDBAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var productList = new List<Product>
                {
                    new Product { Description = "Television", SKU = 4001, Price = 2000 },
                    new Product { Description = "A funny book", SKU = 43221, Price = 19.99 },
                    new Product { Description = "Bowl for Fido", SKU = 123, Price = 40.00 }
                };

                await _productRepository.InsertManyAsync(productList, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return $"'{nameof(InsertOnMongoDBAsync)}()' -> [Exception: {ex.Message}]";
            }

            return $"{nameof(InsertOnMongoDBAsync)}() -> Done!";
        }
    }
}
