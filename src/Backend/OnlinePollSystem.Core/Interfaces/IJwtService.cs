using OnlinePollSystem.Core.Models;

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}