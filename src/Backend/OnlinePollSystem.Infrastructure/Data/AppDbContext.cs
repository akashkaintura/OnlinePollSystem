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

            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne()
                .HasForeignKey(o => o.PollId);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.PollId, v.VoterIdentifier })
                .IsUnique();
        }
    }
}