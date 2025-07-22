using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using WebApi.Models.Entity;
using WebApi.Services.Abstractions;

namespace WebApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IConfiguration _configuration;
        public LoginService(MainDbContext mainDbContext, IConfiguration configuration)
        {
            _mainDbContext = mainDbContext;
            _configuration = configuration;
        }




        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await _mainDbContext.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();

            return user != null && user.Password == password;
        }

        public async Task EnsureTestUserExists()
        {
            if (!await _mainDbContext.Users.AnyAsync(u => u.Username == "user1"))
            {
                _mainDbContext.Users.Add(new User
                {
                    Username = "user1",
                    Password = "Password123"
                });
                await _mainDbContext.SaveChangesAsync();
            }
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            string publicKeyPem = _configuration["Jwt:PublicKey"]!;
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
