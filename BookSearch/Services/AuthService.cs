using BookSearch.Data;
using Microsoft.EntityFrameworkCore;
using BookSearch.Model;
using System.Security.Cryptography;
using System.Text;

namespace BookSearch.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _dbContext;
        public AuthService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> RegisterUser(string username, string password)
        {
            var userExist = await _dbContext.Users.AnyAsync(u => u.UserName == username);
            if (userExist == true)
            {
                return false;
            }
            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var user = new User { UserName = username, PasswordHash = hash, PasswordSalt = salt };
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        private string GenerateSalt()
        {
            // Generate a random salt for password hashing
            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            // Combine the password and salt and hash them
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

    }
}

