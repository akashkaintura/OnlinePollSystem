using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public Poll Poll { get; set; }
        
        public int PollOptionId { get; set; }
        public PollOption PollOption { get; set; }
        
        public int UserId { get; set; }
        public User Voter { get; set; }
        
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}