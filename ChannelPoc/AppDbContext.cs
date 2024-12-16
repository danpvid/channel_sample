
using ChannelPoc.Model;
using Microsoft.EntityFrameworkCore;
namespace ChannelPoc
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; } 
    }

}
