using BookSearch.Data;
using BookSearch.Services;
using Microsoft.EntityFrameworkCore;

namespace BookSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure CORS to allow any origin, method, and header
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()          // Allow any origin
                          .AllowAnyMethod()          // Allow any HTTP method
                          .AllowAnyHeader();         // Allow any headers
                });
            });
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString("DefaultConnection")
               ));
           




            // Add services to the container.
            builder.Services.AddScoped<IBookServices, BookServices>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IFavouriteService, FavouriteService>();
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
            });
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Information);


            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Enable Swagger UI in development environment
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS middleware
            app.UseCors("AllowAllOrigins");

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            // Enable authorization middleware
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Run the app
            app.Run();
        }
    }
}
