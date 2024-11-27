namespace BookSearch.Data
{
    using Microsoft.EntityFrameworkCore;
    using BookSearch.Model;

    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Login> Logins { get; set; }
    }
}
