using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dtos.Login
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
