using Microsoft.EntityFrameworkCore;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.DTOs.Common;
using OnlinePollSystem.Infrastructure.Data;
using OnlinePollSystem.Infrastructure.Repositories;
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
    }
}