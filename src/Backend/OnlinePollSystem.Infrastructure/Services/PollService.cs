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

         public async Task<Vote> SubmitVoteAsync(int userId, Vote vote)
        {
            if (await _pollRepository.HasUser VotedAsync(userId, vote.PollId))
                throw new InvalidOperationException("User  has already voted in this poll");

            return await _pollRepository.SubmitVoteAsync(vote);
        }

        public async Task<PollResultDto> GetPollResultsAsync(int pollId)
        {
            return await _pollRepository.GetPollResultsAsync(pollId);
        }

        public async Task<Poll> CreatePollAsync(PollCreateDto pollCreateDto, int creatorId)
        {
            var poll = new Poll
            {
                Title = pollCreateDto.Title,
                Description = pollCreateDto.Description,
                StartDate = pollCreateDto.StartDate,
                EndDate = pollCreateDto.EndDate,
                CreatorId = creatorId,
                Options = pollCreateDto.Options.Select(option => new PollOption
                {
                    OptionText = option
                }).ToList()
            };

            return await _pollRepository.CreatePollAsync(poll);
        }

        public async Task<Poll> GetPollByIdAsync(int pollId)
        {
            return await _pollRepository.GetPollByIdAsync(pollId);
        }

        public async Task<List<Poll>> GetActivePoolsAsync()
        {
            return await _pollRepository.GetActivePoolsAsync();
        }

    }
}