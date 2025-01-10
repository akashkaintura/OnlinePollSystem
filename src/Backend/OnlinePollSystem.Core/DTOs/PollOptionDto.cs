 using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.DTOs.Poll
{
 public class PollOptionResultDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }
}