using System.Net.Http.Json;
using PollApp.Mobile.Models;
using PollApp.Mobile.Services.Interfaces;
using PollApp.Mobile.Helpers;

namespace PollApp.Mobile.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConnectivity _connectivity;

        public AuthService(
            HttpClient httpClient, 
            IConnectivity connectivity)
        {
            _httpClient = httpClient;
            _connectivity = connectivity;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No internet connection");
            }

            var loginRequest = new 
            {
                email,
                password
            };

            var response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent);
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("No internet connection");
            }

            var registerRequest = new 
            {
                username,
                email,
                password
            };

            var response = await _httpClient.PostAsJsonAsync("auth/register", registerRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent);
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task LogoutAsync()
        {
            // Clear local storage
            await SecureStorage.RemoveAsync("auth_token");
            
            // Optional: Call logout endpoint
            await _httpClient.PostAsync("auth/logout", null);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException("No authenticated user");
            }

            var response = await _httpClient.GetAsync("auth/me");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Failed to retrieve user");
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }
    }
}