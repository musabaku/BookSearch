using Microsoft.EntityFrameworkCore;
using BookSearch.Model;

namespace BookSearch.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Authentication-related tables
        public DbSet<User> Users { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Login> Logins { get; set; }

        // Book-related tables
        public DbSet<BookStorageModel> Books { get; set; }
        public DbSet<FavouriteModel> Favorites { get; set; }

        // Configure the BookId as an auto-incrementing primary key using Fluent API
       
    }
}
