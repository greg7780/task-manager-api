using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Description)
                      .IsRequired();

                entity.Property(e => e.Priority)
                      .IsRequired();

                entity.Property(e => e.IsCompleted)
                      .HasDefaultValue(false);

                entity.Property(e => e.Deleted)
                      .HasDefaultValue(false);

                entity.Property(e => e.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                      .IsRequired();

                entity.Property(e => e.ModifiedBy)
                      .HasMaxLength(255);

                entity.Property(e => e.ModifiedAt);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
