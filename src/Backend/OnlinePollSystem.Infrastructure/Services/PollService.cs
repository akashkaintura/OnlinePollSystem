using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.DTOs;

namespace OnlinePollSystem.Infrastructure.Services
{
    public class PollService : IPollService
    {
        private readonly IPollRepository _pollRepository;

        public PollService(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public async Task<PollResultDto> SubmitVoteAsync(int pollId, int optionId, string voterIdentifier)
        {
            return await _pollRepository.SubmitVoteAsync(pollId, optionId, voterIdentifier);
        }

        public async Task<PollResultDto> GetPollResultsAsync(int pollId)
        {
            return await _pollRepository.GetPollResultsAsync(pollId);
        }
    }
}