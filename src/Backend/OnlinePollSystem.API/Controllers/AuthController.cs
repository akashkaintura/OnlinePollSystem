

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        return result.Success 
            ? Ok(result) 
            : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return result.Success 
            ? Ok(result) 
            : Unauthorized(result);
    }
}