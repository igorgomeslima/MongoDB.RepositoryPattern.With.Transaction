using System.Threading;
using System.Threading.Tasks;

namespace Shared.Application.Common.Interfaces.Services
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
