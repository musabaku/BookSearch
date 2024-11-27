namespace BookSearch.Services
{
    public interface IAuthService
    {
        Task<Boolean> RegisterUser(string username, string password);
    }
}
