using System.Net.Http.Json;
using System.Text.Json;
using PollApp.Mobile.Helpers;
using PollApp.Mobile.Models;

namespace PollApp.Mobile.Services.Implementations
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConnectivity _connectivity;

        public ApiService(HttpClient httpClient, IConnectivity connectivity)
        {
            _httpClient = httpClient;
            _connectivity = connectivity;

            // Configure default headers
            ConfigureDefaultHeaders();
        }

        private void ConfigureDefaultHeaders()
        {
            _httpClient.BaseAddress = new Uri(Settings.Api.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            await EnsureConnectivity();

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error fetching data from {endpoint}", ex);
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            await EnsureConnectivity();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error posting data to {endpoint}", ex);
            }
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            await EnsureConnectivity();

            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, data);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error updating data at {endpoint}", ex);
            }
        }

        public async Task DeleteAsync(string endpoint)
        {
            await EnsureConnectivity();

            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                await HandleResponse(response);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error deleting data at {endpoint}", ex);
            }
        }

        private async Task EnsureConnectivity()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new NoInternetException("No internet connection available.");
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            await EnsureSuccessStatusCode(response);
            return await DeserializeResponse<T>(response);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            await EnsureSuccessStatusCode(response);
        }

        private async Task EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApiException($"API Error: {response.StatusCode}", errorContent);
            }
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // Custom Exceptions
        public class ApiException : Exception
        {
            public string ErrorContent { get; }

            public ApiException(string message, Exception innerException) 
                : base(message, innerException)
            {
            }

            public ApiException(string message, string errorContent) 
                : base(message)
            {
                ErrorContent = errorContent;
            }
        }

        public class NoInternetException : Exception
        {
            public NoInternetException(string message) : base(message) { }
        }
    }
}