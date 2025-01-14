using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlinePollSystem.Domain.Entities
{
    public class PollOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        [Required]
        public int PollId { get; set; }

        public Poll Poll { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        // Computed Properties
        [NotMapped]
        public int VoteCount => Votes.Count;

        [NotMapped]
        public double VotePercentage { get; set; }

        // Methods
        public void UpdateVotePercentage(int totalVotes)
        {
            VotePercentage = totalVotes > 0 
                ? (double)VoteCount / totalVotes * 100 
                : 0;
        }
    }
}