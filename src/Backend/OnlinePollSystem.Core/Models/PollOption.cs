using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.Models
{
    public class PollOption
    {
        public int Id { get; set; }
        
        public int PollId { get; set; }
        public Poll Poll { get; set; }
        
        [Required]
        [StringLength(200)]
        public string OptionText { get; set; }
        
        public int VoteCount { get; set; } = 0;
    }
}