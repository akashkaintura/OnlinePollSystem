using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlinePollSystem.Core.Models
{
    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public List<PollOption> Options { get; set; } = new List<PollOption>();
    }

    public class PollOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; }

        [Required]
        [MaxLength(200)]
        public string OptionText { get; set; }

        public int VoteCount { get; set; }
    }

    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; }

        [Required]
        public int PollOptionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string VoterIdentifier { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}