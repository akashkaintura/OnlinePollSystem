using OnlinePollSystem.Domain.Entities;

namespace OnlinePollSystem.Domain.Interfaces.Repositories
{
    public interface IVoteRepository
    {
        // Basic CRUD Operations
        Task<Vote> GetByIdAsync(int id);
        Task<IEnumerable<Vote>> GetAllAsync();
        Task<Vote> AddAsync(Vote vote);
        Task UpdateAsync(Vote vote);
        Task DeleteAsync(int id);

        // Vote-Specific Queries
        Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId);
        Task<IEnumerable<Vote>> GetVotesByUserIdAsync(int userId);
        Task<IEnumerable<Vote>> GetVotesByPollAndUserAsync(int pollId, int userId);

        // Vote Statistics
        Task<int> GetVoteCountForPollAsync(int pollId);
        Task<int> GetVoteCountForOptionAsync(int optionId);
        Task<Dictionary<int, int>> GetVoteCountsByOptionAsync(int pollId);

        // Validation and Checking
        Task<bool> HasUserVotedInPollAsync(int pollId, int userId);
        Task<int> GetUserVoteCountInPollAsync(int pollId, int userId);

        // Advanced Filtering
        Task<IEnumerable<Vote>> FilterVotesAsync(
            int? pollId = null, 
            int? userId = null, 
            DateTime? startDate = null, 
            DateTime? endDate = null);

        // Bulk Operations
        Task<int> DeleteVotesForPollAsync(int pollId);
        Task<int> DeleteVotesByUserAsync(int userId);

        // Aggregation Methods
        Task<IEnumerable<VoteAggregate>> GetVoteAggregateByPollAsync(int pollId);
    }

    // DTO for vote aggregation
    public class VoteAggregate
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
        public double VotePercentage { get; set; }
    }
}