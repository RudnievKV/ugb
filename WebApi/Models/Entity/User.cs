using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApi.Models.Entity
{
    public class User
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
