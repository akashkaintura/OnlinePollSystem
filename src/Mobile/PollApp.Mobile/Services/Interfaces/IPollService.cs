public interface IPollService
{
    Task<List<Poll>> GetActivePollsAsync();
    Task<Poll> GetPollByIdAsync(int pollId);
    Task<VoteResult> VoteOnPollAsync(int pollId, int optionId);
    Task<Poll> CreatePollAsync(Poll poll);
    Task<List<Poll>> GetUserPollsAsync();
}