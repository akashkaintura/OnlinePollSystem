using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.DTOs.Poll
{
    public class PollResultDto
    {
        public int PollId { get; set; }
        public string Title { get; set; }
        public List<PollOptionResultDto> OptionResults { get; set; }
    }

    public class PollOptionResultDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }
}