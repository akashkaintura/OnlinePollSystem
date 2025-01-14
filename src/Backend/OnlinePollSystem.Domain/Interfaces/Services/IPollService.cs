using OnlinePollSystem.Domain.Entities;
using OnlinePollSystem.Domain.Enums;

namespace OnlinePollSystem.Domain.Interfaces.Services
{
    public interface IPollService
    {
        // Poll Creation and Management
        Task<Poll> CreatePollAsync(Poll poll, List<string> options);
        Task<Poll> UpdatePollAsync(Poll poll);
        Task DeletePollAsync(int pollId);

        // Poll Retrieval
        Task<Poll> GetPollByIdAsync(int pollId);
        Task<IEnumerable<Poll>> GetUserPollsAsync(int userId);
        Task<IEnumerable<Poll>> GetActivePollsAsync();

        // Poll Lifecycle
        Task ActivatePollAsync(int pollId);
        Task ClosePollAsync(int pollId);
        Task ArchivePollAsync(int pollId);

        // Voting Operations
        Task<Vote> CastVoteAsync(int pollId, int userId, int optionId);
        Task<bool> CanUserVoteAsync(int pollId, int userId);

        // Poll Statistics
        Task<PollStatistics> GetPollStatisticsAsync(int pollId);

        // Poll Option Management
        Task<PollOption> AddPollOptionAsync(int pollId, string optionText);
        Task RemovePollOptionAsync(int optionId);

        // Advanced Querying
        Task<IEnumerable<Poll>> SearchPollsAsync(
            string searchTerm, 
            PollStatus? status = null, 
            int? userId = null);
    }

    // DTO for poll statistics
    public class PollStatistics
    {
        public int TotalVotes { get; set; }
        public List<OptionStatistic> OptionStatistics { get; set; }
    }

    public class OptionStatistic
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
        public double VotePercentage { get; set; }
    }
}