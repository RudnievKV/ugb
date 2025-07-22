namespace WebApi.Services.Abstractions
{
    public interface ILoginService
    {
        Task EnsureTestUserExists();
        Task<bool> ValidateCredentials(string username, string password);
        bool ValidateToken(string token);
    }
}
