using OnlinePollSystem.Core.DTOs.Auth;

namespace OnlinePollSystem.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
    }
}