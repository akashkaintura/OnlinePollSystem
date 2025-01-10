using Microsoft.EntityFrameworkCore;
using OnlinePollSystem.Core.Models;

namespace OnlinePollSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) {}

        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // PostgreSQL-specific configuration
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne(o => o.Poll)
                .HasForeignKey(o => o.PollId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Poll)
                .WithMany()
                .HasForeignKey(v => v.PollId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.PollOption)
                .WithMany()
                .HasForeignKey(v => v.PollOptionId);
        }
    }
}