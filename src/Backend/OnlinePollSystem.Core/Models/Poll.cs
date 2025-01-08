using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.Models
{
    public class Poll
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        
        public List<PollOption> Options { get; set; }
    }
}