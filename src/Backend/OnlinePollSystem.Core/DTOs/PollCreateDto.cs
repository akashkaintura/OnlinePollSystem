using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.DTOs.Poll
{
    public class PollCreateDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }
        
        [StringLength(1000)]
        public string Description { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "A poll must have at least two options")]
        public List<string> Options { get; set; }
    }
}