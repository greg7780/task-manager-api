using Microsoft.EntityFrameworkCore;

namespace TaskManagerApi.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
    }
}
