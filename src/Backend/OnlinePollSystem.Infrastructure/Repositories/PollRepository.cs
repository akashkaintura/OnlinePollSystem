using Microsoft.EntityFrameworkCore;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.DTOs.Common;
using OnlinePollSystem.Infrastructure.Data;
using OnlinePollSystem.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;

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

        public async Task<Vote> SubmitVoteAsync(Vote vote)
        {
            // Check if user has already voted
            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == vote.UserId && v.PollId == vote.PollId);

            if (existingVote != null)
            {
                throw new InvalidOperationException("User has already voted in this poll");
            }

            // Add Vote
            _context.Votes.Add(vote);

            // Update vote count for the option
            var option = await _context.PollOptions
                .FirstOrDefaultAsync(o => o.Id == vote.PollOptionId && o.PollId == vote.PollId);

            if (option != null)
            {
                // throw new ArgumentException("Invalid poll option");
                option.VoteCount++;
            }

            await _context.SaveChangesAsync();

            // Return updated poll results
            return vote;
        }

        public async Task<bool> HasUserVotedAsync(int userId, int pollId)
        {
            return await _context.Votes
                .AnyAsync(v => v.UserId == userId && v.PollId == pollId);
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


    public async Task<PaginatedResultDto<Poll>> GetPaginatedPollsAsync(
        PaginationDto paginationDto, 
        bool onlyActive = true)
    {
        var query = _context.Polls
            .Include(p => p.Options)
            .Include(p => p.Creator)
            .AsQueryable();

        if (onlyActive)
        {
            query = query.Where(p => p.IsActive && p.EndDate > DateTime.UtcNow);
        }

        var totalItems = await query.CountAsync();

        var polls = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
            .Take(paginationDto.PageSize)
            .ToListAsync();

        return new PaginatedResultDto<Poll>
        {
            Items = polls,
            TotalItems = totalItems,
            PageNumber = paginationDto.PageNumber,
            PageSize = paginationDto.PageSize
        };
    }

        public async Task<List<Poll>> SearchPollsAsync(string searchTerm)
    {
        return await _context.Polls
            .Include(p => p.Options)
            .Where(p => 
                p.Title.Contains(searchTerm) || 
                p.Description.Contains(searchTerm))
            .ToListAsync();
    }

        public async Task<Poll> GetPollWithOptionsAndVotesAsync(int pollId)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                    .ThenInclude(o => o.Votes)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll != null)
            {
                int totalVotes = poll.Votes.Count;

                // Calculate vote percentages
                foreach (var option in poll.Options)
                {
                    // Use the VoteCount property we added to PollOption
                    option.UpdateVotePercentage(totalVotes);
                }
            }

            return poll;
        }

        public async Task<IEnumerable<Poll>> GetPollStatisticsAsync()
        {
            var polls = await _context.Polls
                .Include(p => p.Options)
                    .ThenInclude(o => o.Votes)
                .ToListAsync();

            foreach (var poll in polls)
            {
                int totalVotes = poll.Votes.Count;

                var pollOptions = poll.Options.Select(option => new 
                {
                    OptionId = option.Id,
                    OptionText = option.Text, // Use Text property
                    VoteCount = option.Votes.Count, // Use Votes collection
                    VotePercentage = totalVotes > 0 
                        ? (double)option.Votes.Count / totalVotes * 100 
                        : 0
                }).ToList();
            }

            return polls;
        }

        // If you need a method returning option statistics
        public async Task<IEnumerable<object>> GetPollOptionStatisticsAsync(int pollId)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                    .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
                return new List<object>();

            int totalVotes = poll.Votes.Count;

            return PollOptionStatisticsDto.FromPollOptions(poll.Options, totalVotes);
        }
    }
}