using Microsoft.EntityFrameworkCore;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;
using OnlinePollSystem.Core.DTOs;
using OnlinePollSystem.Infrastructure.Data;

namespace OnlinePollSystem.Infrastructure.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly AppDbContext _context;

        public PollRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Poll> CreatePollAsync(PollCreateDto pollDto)
        {
            var poll = new Poll
            {
                Title = pollDto.Title,
                Description = pollDto.Description,
                StartDate = pollDto.StartDate,
                EndDate = pollDto.EndDate,
                IsActive = true,
                Options = pollDto.Options.Select(o => new PollOption 
                { 
                    OptionText = o,
                    VoteCount = 0
                }).ToList()
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return poll;
        }

        public async Task<Poll> GetPollByIdAsync(int pollId)
        {
            return await _context.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == pollId);
        }

        public async Task<List<Poll>> GetActivePoolsAsync()
        {
            return await _context.Polls
                .Include(p => p.Options)
                .Where(p => p.IsActive && p.EndDate > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<PollResultDto> SubmitVoteAsync(int pollId, int optionId, string voterIdentifier)
        {
            // Check if user has already voted
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.PollId == pollId && v.VoterIdentifier == voterIdentifier);

            if (existingVote != null)
            {
                throw new InvalidOperationException("User has already voted in this poll");
            }

            // Add new vote
            var vote = new Vote
            {
                PollId = pollId,
                PollOptionId = optionId,
                VoterIdentifier = voterIdentifier,
                VotedAt = DateTime.UtcNow
            };

            _context.Votes.Add(vote);

            // Update vote count for the option
            var option = await _context.PollOptions
                .FirstOrDefaultAsync(o => o.Id == optionId && o.PollId == pollId);

            if (option == null)
            {
                throw new ArgumentException("Invalid poll option");
            }

            option.VoteCount++;

            await _context.SaveChangesAsync();

            // Return updated poll results
            return await GetPollResultsAsync(pollId);
        }

        public async Task<PollResultDto> GetPollResultsAsync(int pollId)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
            {
                throw new ArgumentException("Poll not found");
            }

            var totalVotes = poll.Options.Sum(o => o.VoteCount);

            return new PollResultDto
            {
                PollId = poll.Id,
                Title = poll.Title,
                OptionResults = poll.Options.Select(o => new PollOptionResultDto
                {
                    OptionId = o.Id,
                    OptionText = o.OptionText,
                    VoteCount = o.VoteCount,
                    Percentage = totalVotes > 0 ? (double)o.VoteCount / totalVotes * 100 : 0
                }).ToList()
            };
        }
    }
}