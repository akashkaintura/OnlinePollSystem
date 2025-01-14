using System.Text.Json.Serialization;

namespace OnlinePollSystem.Domain.Dtos
{
    public class PollOptionStatisticsDto
    {
        [JsonPropertyName("optionId")]
        public int OptionId { get; set; }

        [JsonPropertyName("optionText")]
        public string OptionText { get; set; }

        [JsonPropertyName("voteCount")]
        public int VoteCount { get; set; }

        [JsonPropertyName("votePercentage")]
        public double VotePercentage { get; set; }

        // Optional: Computed property
        [JsonPropertyName("isLeading")]
        public bool IsLeading { get; set; }

        // Mapping method from domain entity
        public static PollOptionStatisticsDto FromPollOption(PollOption option, int totalVotes)
        {
            return new PollOptionStatisticsDto
            {
                OptionId = option.Id,
                OptionText = option.Text,
                VoteCount = option.Votes.Count,
                VotePercentage = totalVotes > 0 
                    ? (double)option.Votes.Count / totalVotes * 100 
                    : 0
            };
        }

        // Bulk mapping method
        public static List<PollOptionStatisticsDto> FromPollOptions(
            ICollection<PollOption> options, 
            int totalVotes)
        {
            var statistics = options
                .Select(option => FromPollOption(option, totalVotes))
                .ToList();

            // Determine leading option
            if (statistics.Any())
            {
                var maxVotes = statistics.Max(s => s.VoteCount);
                statistics.ForEach(s => 
                    s.IsLeading = s.VoteCount == maxVotes);
            }

            return statistics;
        }
    }
}