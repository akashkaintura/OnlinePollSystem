using PollApp.Mobile.Models;

namespace PollApp.Mobile.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> RegisterAsync(string username, string email, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<User> GetCurrentUserAsync();
    }
}