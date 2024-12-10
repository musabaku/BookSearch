using BookSearch.Data;
using Microsoft.EntityFrameworkCore;
using BookSearch.Model;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace BookSearch.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthService(ApplicationDbContext dbContext)
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
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<String> LoginUser(string username, string password)
        {
            if(string.IsNullOrWhiteSpace(username)|| string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException ("Provide proper username and password");
            }
            var userRetrieve = await _dbContext.Users.FirstOrDefaultAsync(u=>u.UserName == username);
            if (userRetrieve == null)
            {
                throw new InvalidOperationException("User not found create an account");

            }
            var salt = userRetrieve.PasswordSalt;
            var hash = HashPassword(password, salt);
            if (!hash.Equals(userRetrieve.PasswordHash))
            {
                throw new InvalidOperationException("Password dont match, Incorrect password");

            }
            var token = GenerateJWTToken(userRetrieve.UserId);
            return token;
        }
        public String GenerateJWTToken(int UserId)
        {
            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub,UserName)
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()),
            };
            var secretKey = "your_super_secure_256bit_secret_key_here_12345678"; // 32 characters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token expires in 1 day
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

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

