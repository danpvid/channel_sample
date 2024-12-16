using ChannelPoc.Model;

namespace ChannelPoc.BackgroundServices.Job
{
    public record ImportClientsJob(Guid Id, Client[] Clients);

    public enum ImportClientsStatus
    {
        Queued,
        Running,
        Completed,
        Failed
    }
}
