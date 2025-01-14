using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlinePollSystem.Domain.Enums;

namespace OnlinePollSystem.Domain.Entities
{
    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public PollStatus Status { get; set; } = PollStatus.Draft;

        public bool IsAnonymous { get; set; } = false;
        public bool AllowMultipleVotes { get; set; } = false;

        // Navigation Properties
        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        // Computed Properties
        [NotMapped]
        public int TotalVotes => Votes.Count;

        [NotMapped]
        public bool IsActive => Status == PollStatus.Active && 
                                DateTime.UtcNow >= StartDate && 
                                DateTime.UtcNow <= EndDate;

        // Methods
        public void Activate()
        {
            Status = PollStatus.Active;
            StartDate = DateTime.UtcNow;
        }

        public void Close()
        {
            Status = PollStatus.Closed;
            EndDate = DateTime.UtcNow;
        }

        public void Archive()
        {
            Status = PollStatus.Archived;
        }

        public PollOption AddOption(string optionText)
        {
            var option = new PollOption
            {
                Text = optionText,
                PollId = Id
            };
            Options.Add(option);
            return option;
        }
    }
}