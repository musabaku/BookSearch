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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BookStorageModel
            modelBuilder.Entity<BookStorageModel>(entity =>
            {
                entity.HasKey(e => e.BookId);  // Set BookId as primary key
                entity.Property(e => e.BookId)  // Make BookId auto-increment
                      .ValueGeneratedOnAdd()  // Auto-increment on insert
                      .IsRequired();  // Mark it as required (not nullable)
            });

            // Additional configurations for other models can go here
        }
    }
}
