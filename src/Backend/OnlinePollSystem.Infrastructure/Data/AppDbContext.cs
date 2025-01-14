using Microsoft.EntityFrameworkCore;
using OnlinePollSystem.Core.Models;

namespace OnlinePollSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) {}
        
        // Add DbSet for Users
        public DbSet<User> Users { get; set; }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // PostgreSQL-specific configuration
            modelBuilder.HasPostgresExtension("uuid-ossp");

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
            });

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

            modelBuilder.Entity<PollOption>()
            .HasMany(o => o.Votes)
            .WithOne(v => v.Option)
            .HasForeignKey(v => v.OptionId);

            modelBuilder.Entity<Poll>()
            .HasMany(p => p.Options)
            .WithOne(o => o.Poll)
            .HasForeignKey(o => o.PollId);

            // modelBuilder.Entity<Poll>()
            //     .HasOne(v => v.Poll)
            //     .WithMany()
            //     .HasForeignKey(v => v.PollId);

            // modelBuilder.Entity<PollOption>()
            //     .HasOne(v => v.PollOption)
            //     .WithMany()
            //     .HasForeignKey(v => v.PollOptionId);
        }
    }
}