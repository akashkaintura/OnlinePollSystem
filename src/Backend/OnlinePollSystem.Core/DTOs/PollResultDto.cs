using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.DTOs.Poll
{
    public class PollResultDto
    {
        public int PollId { get; set; }
        public string Title { get; set; }
        public List<PollOptionResultDto> OptionResults { get; set; }
    }

   
}