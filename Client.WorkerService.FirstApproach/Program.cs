using Shared;
using Shared.Application.Services;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Client.WorkerService.FirstApproach.Scoped;
using Shared.Infrastructure.Persistence.DbContexts;
using Shared.Application.Common.Interfaces.Services;
using Shared.Application.Common.Interfaces.DbContext;
using Shared.Application.Common.Interfaces.Repository;
using Shared.Infrastructure.Persistence.Repository.MongoDB.Mappings;
using Shared.Infrastructure.Persistence.Repository.MongoDB._Approachs.First;

namespace Client.WorkerService.FirstApproach
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

                    services.AddScoped<IMyService, MyServiceFirstApproach>();
                    services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

                    services.AddScoped<IProductRepository, ProductRepositoryMongoDBFirstApproach>();
                    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryMongoDBFirstApproach<>));

                    MongoMappings.Configure();
                    services.AddScoped<IMongoDbContextFirstApproach, MongoDbContextFirstApproach>();

                    services.AddHostedService<ScopedWorker>();
                });
    }
}
