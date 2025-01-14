using OnlinePollSystem.Domain.Entities;

namespace OnlinePollSystem.Domain.Interfaces.Services
{
    public interface IUserService
    {
        // User Authentication
        Task<User> RegisterUserAsync(string username, string email, string password);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        // User Management
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> UpdateUserProfileAsync(int userId, string username, string email);
        Task DeactivateUserAsync(int userId);
        Task ReactivateUserAsync(int userId);

        // User Statistics
        Task<UserStatistics> GetUserStatisticsAsync(int userId);

        // Password Recovery
        Task<bool> InitiatePasswordResetAsync(string email);
        Task<bool> CompletePasswordResetAsync(string resetToken, string newPassword);

        // Account Verification
        Task<bool> SendVerificationEmailAsync(int userId);
        Task<bool> VerifyEmailAsync(string verificationToken);

        // Advanced User Queries
        Task<IEnumerable<User>> SearchUsersAsync(
            string searchTerm, 
            bool? isActive = null, 
            int pageNumber = 1, 
            int pageSize = 10);
    }

    // DTO for user statistics
    public class UserStatistics
    {
        public int TotalPollsCreated { get; set; }
        public int TotalVotesCast { get; set; }
        public DateTime LastLoginAt { get; set; }
        public List<PollSummary> RecentPolls { get; set; }
    }

    public class PollSummary
    {
        public int PollId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalVotes { get; set; }
    }
}