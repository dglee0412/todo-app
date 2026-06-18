using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique(); //이메일 중복 방지
                entity.Property(u => u.Email).HasMaxLength(256);
                entity.Property(u => u.DisplayName).HasMaxLength(100);
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.Property(t => t.Title).HasMaxLength(200);

                entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
                entity.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);

                entity.HasOne(t => t.User)
                      .WithMany(u => u.Todos)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        //protected AppDbContext()
        //{
        //}
    }
}
