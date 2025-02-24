using System.ComponentModel.DataAnnotations;

namespace OnlinePollSystem.Core.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }
    }
}