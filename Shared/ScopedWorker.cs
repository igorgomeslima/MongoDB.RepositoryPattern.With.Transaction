using System;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Common.Interfaces.Services;

namespace Shared
{
    public class ScopedWorker : BackgroundService
    {
        const string SCOPED_SERVICE_MESSAGE = "Consume Scoped Service Hosted Service";

        public IServiceProvider Services { get; }

        public ScopedWorker(IServiceProvider services, ILogger<ScopedWorker> logger)
        {
            _logger = logger;
            Services = services;
        }

        private readonly ILogger<ScopedWorker> _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{SCOPED_SERVICE_MESSAGE} running -> {Assembly.GetEntryAssembly().FullName}.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{SCOPED_SERVICE_MESSAGE} is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService = 
                    scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{SCOPED_SERVICE_MESSAGE} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
