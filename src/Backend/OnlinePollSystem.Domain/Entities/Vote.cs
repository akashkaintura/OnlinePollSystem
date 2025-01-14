using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlinePollSystem.Domain.Entities
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int PollId { get; set; }
        public Poll Poll { get; set; }

        [Required]
        public int OptionId { get; set; }
        public PollOption Option { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        // Additional metadata
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }

        // Methods
        public bool IsValidVote()
        {
            return Poll.IsActive && 
                   (Poll.AllowMultipleVotes || 
                    !Poll.Votes.Any(v => v.UserId == UserId));
        }
    }
}