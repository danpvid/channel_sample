
using ChannelPoc.BackgroundServices.Job;
using ChannelPoc.Controllers;
using ChannelPoc.Services;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace ChannelPoc.BackgroundServices
{
    public class ImportClientBackgroundService(
        ILogger<ClientController> _logger,
        IServiceProvider _serviceProvider,
        Channel<ImportClientsJob> _channel,
        ConcurrentDictionary<Guid, ImportClientsStatus> _status
        ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var job in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    _status[job.Id] = ImportClientsStatus.Running;
                    using var scope = _serviceProvider.CreateScope();
                    var scopedService = scope.ServiceProvider.GetRequiredService<ICLientService>();
                    await scopedService.ImportClients(job.Clients);
                    _status[job.Id] = ImportClientsStatus.Completed;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro for importing clients");
                }
            }
        }
    }
}
