using ChannelPoc.Model;
using Microsoft.EntityFrameworkCore;

namespace ChannelPoc.Services
{
    public interface ICLientService
    {
        Task ImportClients(Client[] clients);
        Task<IEnumerable<Client>> GetClients(Guid importId);
    }

    public class ClientService(AppDbContext context) : ICLientService
    {
        public async Task ImportClients(Client[] clients)
        {
            await context.Clients.AddRangeAsync(clients);
            context.SaveChanges();
            await Task.Delay(20_000);
        }

        public async Task<IEnumerable<Client>> GetClients(Guid importId) =>
            await context.Clients.Where(x => x.ImportId == importId).ToListAsync();
    }
}
