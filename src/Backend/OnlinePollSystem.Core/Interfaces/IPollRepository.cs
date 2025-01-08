using System.Threading.Tasks;
using System.Collections.Generic;
using OnlinePollSystem.Core.Models;
using OnlinePollSystem.Core.DTOs; 

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IPollRepository
    {
        /// <summary>
        /// Create a new poll
        /// </summary>
        /// <param name="pollDto">Poll creation details</param>
        /// <returns>Created poll</returns>
        Task<Poll> CreatePollAsync(PollCreateDto pollDto);

        /// <summary>
        /// Get a poll by its ID
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <returns>Poll details</returns>
        Task<Poll> GetPollByIdAsync(int pollId);

        /// <summary>
        /// Get all active polls
        /// </summary>
        /// <returns>List of active polls</returns>
        Task<List<Poll>> GetActivePoolsAsync();

        /// <summary>
        /// Submit a vote for a poll
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <param name="optionId">The ID of the selected option</param>
        /// <param name="voterIdentifier">Unique identifier for the voter</param>
        /// <returns>Updated poll results</returns>
        Task<PollResultDto> SubmitVoteAsync(int pollId, int optionId, string voterIdentifier);

        /// <summary>
        /// Get poll results
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <returns>Poll results</returns>
        Task<PollResultDto> GetPollResultsAsync(int pollId);
    }
}