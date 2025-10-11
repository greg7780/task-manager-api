using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Handlers;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                      .IsRequired();

                entity.Property(e => e.Deleted)
                      .HasDefaultValue(false);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.Expires).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>().HasData([
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "AQAAAAIAACcQAAAAEKsV+Q9q+HiqBHRwdK9JBk3e9aPPV/PB9ZnkhpgHPdy93kHI1dqHV5g+M0MwRXWyog==",
                    Deleted = false
                }
            ]);

            base.OnModelCreating(modelBuilder);
        }
    }
}
