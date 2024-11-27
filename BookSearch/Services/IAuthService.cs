namespace BookSearch.Services
{
    public interface IAuthService
    {
        Task<Boolean> RegisterUser(string username, string password);
        Task<String> LoginUser(string username, string password);
    }
}
