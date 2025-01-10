using System.Collections.Generic;
using System.Threading.Tasks;
using OnlinePollSystem.Core.DTOs.Common;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;
using OnlinePollSystem.Infrastructure.Services;

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
            // Check if user has already voted
            if (await _pollRepository.HasUserVotedAsync(userId, vote.PollId))
            {
                throw new InvalidOperationException("User has already voted in this poll");
            }

            // Set user ID for the vote
            vote.UserId = userId;

            // Submit the vote
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
        
        public async Task<PaginatedResultDto<Poll>> GetPaginatedPollsAsync(PaginationDto paginationDto)
        {
            return await _pollRepository.GetPaginatedPollsAsync(paginationDto);
        }

        public async Task<List<Poll>> SearchPollsAsync(string searchTerm)
        {
            return await _pollRepository.SearchPollsAsync(searchTerm);
        }

        public async Task<bool> HasUserVotedAsync(int userId, int pollId)
        {
            return await _pollRepository.HasUserVotedAsync(userId, pollId);
        }
    }
}

