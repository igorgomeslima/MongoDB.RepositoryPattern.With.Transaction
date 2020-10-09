using Shared;
using Shared.Application.Services;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Client.WorkerService.SecondApproach.Scoped;
using Shared.Infrastructure.Persistence.DbContexts;
using Shared.Infrastructure.Persistence.UnitOfWork;
using Shared.Application.Common.Interfaces.Services;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.Repository;
using Shared.Application.Common.Interfaces.UnitOfWork;
using Shared.Infrastructure.Persistence.Repository.MongoDB.Mappings;
using Shared.Infrastructure.Persistence.Repository.MongoDB._Approachs.Second;

namespace Client.WorkerService.SecondApproach
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<MongoDBSettings>(hostContext.Configuration.GetSection(nameof(MongoDBSettings)));

                    services.AddScoped<IMyService, MyServiceSecondApproach>();
                    services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

                    services.AddScoped<IProductRepository, ProductRepositoryMongoDBSecondApproach>();
                    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryMongoDBSecondApproach<>));

                    MongoMappings.Configure();
                    services.AddScoped<IUnitOfWork, UnitOfWorkMongoDB>();
                    services.AddScoped<IMongoDbContextSecondApproach, MongoDbContextSecondApproach>();

                    services.AddHostedService<ScopedWorker>();
                });
    }
}
