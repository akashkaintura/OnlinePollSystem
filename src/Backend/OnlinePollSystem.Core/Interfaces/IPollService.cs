using System.Threading.Tasks;
using OnlinePollSystem.Core.DTOs;

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IPollService
    {
        /// <summary>
        /// Submit a vote for a specific poll option
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <param name="optionId">The ID of the selected option</param>
        /// <param name="voterIdentifier">Unique identifier for the voter</param>
        /// <returns>Updated poll results</returns>
        Task<PollResultDto> SubmitVoteAsync(int pollId, int optionId, string voterIdentifier);

        /// <summary>
        /// Retrieve results for a specific poll
        /// </summary>
        /// <param name="pollId">The ID of the poll</param>
        /// <returns>Poll results</returns>
        Task<PollResultDto> GetPollResultsAsync(int pollId);
    }
}