using System.Collections.Generic;
namespace OnlinePollSystem.Core.DTOs
{
    public class PollCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Options { get; set; } = new List<string>();
    }
}