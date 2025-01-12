using System.Text.Json.Serialization;

namespace PollApp.Mobile.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [JsonPropertyName("registeredAt")]
        public DateTime RegisteredAt { get; set; }

        [JsonPropertyName("lastLogin")]
        public DateTime? LastLogin { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [JsonPropertyName("pollsCreated")]
        public int PollsCreated { get; set; }

        [JsonPropertyName("totalVotes")]
        public int TotalVotes { get; set; }

        // Computed Properties
        public bool IsAdmin => Roles.Contains("Admin");
        public bool IsActive { get; set; } = true;

        // Method to update user profile
        public void UpdateProfile(string username, string email, string profileImageUrl)
        {
            Username = username;
            Email = email;
            ProfileImageUrl = profileImageUrl;
        }
    }
}