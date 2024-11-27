namespace BookSearch.Data;
using Microsoft.EntityFrameworkCore;
using BookSearch.Model;

    public class AuthDbContext : DbContext
    {

        public DbSet<User> Users => Set<User>();
        public DbSet<Register> Registers => Set<Register>();
        public DbSet<Login> Logins => Set<Login>();
    }
