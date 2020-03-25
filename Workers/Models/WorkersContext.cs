using Microsoft.EntityFrameworkCore;

namespace Workers.Models
{
    public class WorkersContext : DbContext
    {
        public WorkersContext(DbContextOptions<WorkersContext> options) : base(options)
        {
        }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}