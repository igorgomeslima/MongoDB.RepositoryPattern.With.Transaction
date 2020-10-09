
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Application.Common.Interfaces.Services;

namespace Client.WorkerService.FirstApproach.Scoped
{
    internal class ScopedProcessingService : IScopedProcessingService
    {
        int executionCount = 0;
        readonly ILogger _logger;
        readonly IMyService _myServiceFirstApproach;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, IMyService myServiceFirstApproach)
        {
            _logger = logger;
            _myServiceFirstApproach = myServiceFirstApproach;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var insertOnMongoResult = await _myServiceFirstApproach.InsertOnMongoDBAsync(stoppingToken);

                executionCount++;

                _logger.LogInformation(
                    "Result of {insertOnMongoResult} | Execution Count: {Count}.", insertOnMongoResult, executionCount);

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
