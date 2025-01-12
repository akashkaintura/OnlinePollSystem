public class PollService : IPollService
{
    private readonly HttpClient _httpClient;
    private readonly IConnectivity _connectivity;

    public PollService(
        HttpClient httpClient, 
        IConnectivity connectivity)
    {
        _httpClient = httpClient;
        _connectivity = connectivity;
    }

    public async Task<List<Poll>> GetActivePollsAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            // Implement offline strategy
            throw new Exception("No internet connection");
        }

        var response = await _httpClient.GetAsync("polls/active");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Poll>>();
    }

    public async Task<Poll> GetPollByIdAsync(int pollId)
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            // Implement offline strategy
            throw new Exception("No internet connection");
        }

        var response = await _httpClient.GetAsync($"polls/{pollId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Poll>();
    }
}