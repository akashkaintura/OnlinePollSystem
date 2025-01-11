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

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(v => v.PollOption)
                    .WithMany(po => po.Votes)
                    .HasForeignKey(v => v.PollOptionId);
                    
                entity.HasOne(v => v.Poll)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(v => v.PollId);
                    
                entity.HasOne(v => v.Users)
                    .WithMany(u => u.Votes)
                    .HasForeignKey(v => v.UserId);
            });


            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne(o => o.Poll)
                .HasForeignKey(o => o.PollId);

            modelBuilder.Entity<Poll>()
                .HasOne(v => v.Poll)
                .WithMany()
                .HasForeignKey(v => v.PollId);

            modelBuilder.Entity<PollOption>()
                .HasOne(v => v.PollOption)
                .WithMany()
                .HasForeignKey(v => v.PollOptionId);
        }
    }
}