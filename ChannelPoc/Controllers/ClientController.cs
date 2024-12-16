using ChannelPoc.BackgroundServices.Job;
using ChannelPoc.Model;
using ChannelPoc.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace ChannelPoc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController(
        ILogger<ClientController> _logger,
        ICLientService _service,
        Channel<ImportClientsJob> _channel,
        ConcurrentDictionary<Guid, ImportClientsStatus> _status
        ) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var id = Guid.NewGuid();

            var clientes = MockClients(id).ToArray();

            var job = new ImportClientsJob(id, clientes);

            await _channel.Writer.WriteAsync(job);

            _status[id] = ImportClientsStatus.Queued;

            string uri = GetUri(id);

            return Accepted(uri, new { Id = id, Status = $"{ImportClientsStatus.Queued}" });
        }

        [HttpGet("{id}/status")]
        public async Task<ActionResult<IEnumerable<Client>>> Get([FromRoute] Guid id)
        {
            if (!_status.TryGetValue(id, out var status))
                return NotFound();

            if (status != ImportClientsStatus.Completed)
                return Ok(new { Id = id, Status = status });

            var clients = await _service.GetClients(id);

            return Ok(new { Id = id, Status = status, Clients = clients });
        }

        [HttpGet("executions")]
        public ActionResult<IEnumerable<Client>> Get()
        {           
            return Ok(_status.ToDictionary(x => x.Key, y => $"{y.Value}"));
        }

        private static IEnumerable<Client> MockClients(Guid importId) =>
            Enumerable.Range(1, 30).Select(index => new Client(Guid.NewGuid(), $"Cliente {index}", importId));

        private string GetUri(Guid id) =>  $"{Request.Scheme}://{Request.Host}/Client/{id}/status";

    }
}
