using OnlinePollSystem.Core.Models;

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IJwtService  // Changed from JwtService to IJwtService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}

