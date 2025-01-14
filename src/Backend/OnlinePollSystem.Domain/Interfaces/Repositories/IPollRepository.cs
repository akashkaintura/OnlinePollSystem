using OnlinePollSystem.Domain.Entities;
using OnlinePollSystem.Domain.Enums;

namespace OnlinePollSystem.Domain.Interfaces.Repositories
{
    public interface IPollRepository
    {
        // Basic CRUD Operations
        Task<Poll> GetByIdAsync(int id);
        Task<IEnumerable<Poll>> GetAllAsync();
        Task<Poll> AddAsync(Poll poll);
        Task UpdateAsync(Poll poll);
        Task DeleteAsync(int id);

        // Specific Poll Queries
        Task<IEnumerable<Poll>> GetPollsByUserIdAsync(int userId);
        Task<IEnumerable<Poll>> GetActivePollsAsync();
        Task<IEnumerable<Poll>> GetPollsByStatusAsync(PollStatus status);

        // Poll Filtering and Pagination
        Task<IEnumerable<Poll>> GetPollsWithPaginationAsync(
            int pageNumber, 
            int pageSize, 
            string searchTerm = null);

        // Poll Statistics
        Task<int> GetTotalPollCountAsync();
        Task<int> GetActivePollCountAsync();

        Task<List<PollOptionStatisticsDto>> GetPollOptionStatisticsAsync(int pollId);


        // Poll Option Management
        Task AddPollOptionAsync(PollOption option);
        Task RemovePollOptionAsync(int optionId);

        // Advanced Queries
        Task<Poll> GetPollWithOptionsAndVotesAsync(int pollId);
        Task<IEnumerable<Poll>> GetRecentPollsAsync(int count);

        // Poll Lifecycle Management
        Task ActivatePollAsync(int pollId);
        Task ClosePollAsync(int pollId);
        Task ArchivePollAsync(int pollId);

        // Voting Operations
        Task<bool> CanUserVoteAsync(int pollId, int userId);
        Task<IEnumerable<Poll>> GetUpcomingPollsAsync();
        Task<IEnumerable<Poll>> GetExpiredPollsAsync();

        // Complex Filtering
        Task<IEnumerable<Poll>> FilterPollsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            PollStatus? status = null,
            int? userId = null);

        // Bulk Operations
        Task<int> BulkDeleteExpiredPollsAsync();
        Task<int> BulkArchivePollsAsync(DateTime olderThan);
    }
}