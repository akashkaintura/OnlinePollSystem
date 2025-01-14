using System.Threading.Tasks;
using OnlinePollSystem.Core.DTOs;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.Models;
using System.Collections.Generic;
using OnlinePollSystem.Core.DTOs.Common;

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IPollService
    {

        /// <summary>
        /// Create a new poll
        /// </summary>
        /// <param name="poll">Poll details</param>
        /// <returns>Created poll details</returns>
        Task<Poll> CreatePollAsync(PollCreateDto pollCreateDto, int creatorId);

        /// <summary>
        /// Get all active polls by Ids
        /// </summary>
        /// <returns>List of active polls</returns>
        Task<List<Poll>> GetActivePoolsAsync();

        /// <summary>
        /// Get a poll by its ID
        /// </summary>
        /// <param name="id">Poll ID</param>
        /// <returns>Poll details</returns>
        Task<Poll> GetPollByIdAsync(int pollId);

        /// <summary>
        /// Retrieve results for a specific poll
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <returns>Poll results</returns>
        Task<PollResultDto> GetPollResultsAsync(int pollId);

        /// <summary>
        /// Submit a vote for a specific poll option
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <param name="optionId">The ID of the selected option</param>
        /// <param name="voterIdentifier">Unique identifier for the voter</param>
        /// <returns>Updated poll results</returns>
        Task<Vote> VoteAsync(int UserId, Vote vote);

        /// <summary>
        /// has user voted
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="pollId">The ID of the poll</param>
        /// <returns>True if the user has voted, false otherwise</returns>
        Task<bool> HasUserVotedAsync(int userId, int pollId);

        /// <summary>
        /// Paginate all polls
        /// </summary>
        /// <param name="paginationDto">Pagination details</param>
        Task<PaginatedResultDto<Poll>> GetPaginatedPollsAsync(PaginationDto paginationDto);

        /// <summary>
        /// Search for polls by title
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of polls</returns>
        Task<List<Poll>> SearchPollsAsync(string searchTerm);
    }
}