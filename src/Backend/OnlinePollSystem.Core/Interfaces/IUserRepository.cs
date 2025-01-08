using OnlinePollSystem.Core.Models;

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<bool> CheckPasswordAsync(string email, string password);
        Task<User> GetByIdAsync(int id);
    }
}