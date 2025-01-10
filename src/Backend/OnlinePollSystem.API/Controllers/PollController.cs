using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlinePollSystem.Core.DTOs.Poll;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.Models;

namespace OnlinePollSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePoll([FromBody] PollCreateDto pollCreateDto)
        {
            var creatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var poll = await _pollService.CreatePollAsync(pollCreateDto, creatorId);
            return CreatedAtAction(nameof(GetPollById), new { pollId = poll.Id }, poll);
        }

        [HttpGet("{pollId}")]
        public async Task<IActionResult> GetPollById(int pollId)
        {
            var poll = await _pollService.GetPollByIdAsync(pollId);
            if (poll == null) return NotFound();
            return Ok(poll);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePolls()
        {
            var polls = await _pollService.GetActivePollsAsync();
            return Ok(polls);
        }

        [HttpGet("{pollId}/results")]
        public async Task<IActionResult> GetPollResults(int pollId)
        {
            var results = await _pollService.GetPollResultsAsync(pollId);
            if (results == null) return NotFound();
            return Ok(results);
        }

        [HttpPost("{pollId}/vote")]
        [Authorize]
        public async Task<IActionResult> SubmitVote(int pollId, [FromBody] Vote vote)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            vote.PollId = pollId;
            vote.UserId = userId;

            try
            {
                var submittedVote = await _pollService.SubmitVoteAsync(userId, vote);
                return Ok(submittedVote);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedPolls(
            [FromQuery] PaginationDto paginationDto)
        {
            var polls = await _pollService.GetPaginatedPollsAsync(paginationDto);
            return Ok(polls);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPolls([FromQuery] string searchTerm)
        {
            var polls = await _pollService.SearchPollsAsync(searchTerm);
            return Ok(polls);
        }
    }
}