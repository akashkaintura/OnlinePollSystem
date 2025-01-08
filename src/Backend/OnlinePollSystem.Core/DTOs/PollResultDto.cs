using System.Collections.Generic;

namespace OnlinePollSystem.Core.DTOs
{
    public class PollResultDto
    {
        public int PollId { get; set; }
        public string Title { get; set; }
        public List<PollOptionResultDto> OptionResults { get; set; } = new List<PollOptionResultDto>();
    }

    public class PollOptionResultDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }
}