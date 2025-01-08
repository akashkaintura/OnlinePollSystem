namespace OnlinePollSystem.Core.DTOs.Auth
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public object User { get; set; }

        public static AuthResultDto Success(string token, object user) => new()
        {
            Success = true,
            Token = token,
            User = user
        };

        public static AuthResultDto Failed(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}