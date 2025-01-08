using Microsoft.AspNetCore.Mvc;
using OnlinePollSystem.Core.Interfaces;
using OnlinePollSystem.Core.DTOs;

namespace OnlinePollSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService ?? throw new ArgumentNullException(nameof(pollService));
        }

        [HttpPost("{pollId}/vote")]
        public async Task<ActionResult<PollResultDto>> SubmitVote(
            int pollId, 
            [FromBody] VoteSubmissionDto? voteDto)
        {
            if (voteDto == null)
            {
                return BadRequest("Vote submission data is required.");
            }

            var result = await _pollService.SubmitVoteAsync(
                pollId, 
                voteDto.OptionId, 
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            );
            return Ok(result);
        }

        [HttpGet("{pollId}/results")]
        public async Task<ActionResult<PollResultDto>> GetPollResults(int pollId)
        {
            var results = await _pollService.GetPollResultsAsync(pollId);
            return Ok(results);
        }
    }

    public class VoteSubmissionDto
    {
        public int OptionId { get; set; }
    }
}