////for local dev
//using BookSearch.Data;
//using BookSearch.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace BookSearch
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Configure CORS to allow any origin, method, and header
//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAllOrigins", policy =>
//                {
//                    policy.AllowAnyOrigin()          // Allow any origin
//                          .AllowAnyMethod()          // Allow any HTTP method
//                          .AllowAnyHeader();         // Allow any headers
//                });
//            });

//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//               options.UseNpgsql(
//                   builder.Configuration.GetConnectionString("DefaultConnection")
//               ));

//            // Add services to the container.
//            builder.Services.AddScoped<IBookServices, BookServices>();
//            builder.Services.AddScoped<IAuthService, AuthService>();
//            builder.Services.AddScoped<IFavouriteService, FavouriteService>();
//            builder.Services.AddLogging(config =>
//            {
//                config.AddConsole();
//            });
//            builder.Logging.ClearProviders();
//            builder.Logging.AddConsole();
//            builder.Logging.SetMinimumLevel(LogLevel.Information);

//// Add Authentication and JWT Bearer configuration
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            // Validate the token's signature
//            ValidateIssuerSigningKey = true,

//            // Set the key used to sign the token
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secure_256bit_secret_key_here_12345678")),

//            // Validate the token's expiration
//            ValidateLifetime = true,

//            // These are optional since you're not using them
//            ValidateIssuer = false,
//            ValidateAudience = false,
//        };
//    });

//            // Configure Swagger/OpenAPI
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen(options =>
//            {
//                // Add Bearer Token to Swagger
//                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                {
//                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
//                    Name = "Authorization",
//                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
//                });

//                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//                {
//                    {
//                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                        {
//                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                            {
//                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            }
//                        },
//                        new string[] {}
//                    }
//                });
//            });

//            builder.Services.AddControllers();
//            builder.Services.AddHttpClient();

//            var app = builder.Build();

//            // Enable Swagger UI in development environment
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            // Enable CORS middleware
//            app.UseCors("AllowAllOrigins");

//            // Enable HTTPS redirection
//            app.UseHttpsRedirection();

//            // Enable authentication middleware
//            app.UseAuthentication();

//            // Enable authorization middleware
//            app.UseAuthorization();

//            // Map controllers
//            app.MapControllers();

//            // Run the app
//            app.Run();
//        }
//    }
//}

using BookSearch.Data;
using BookSearch.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                options.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()        // Allow any origin
                          .AllowAnyMethod()        // Allow any HTTP method
                          .AllowAnyHeader();       // Allow any headers
                });
            });

            // Configure database connection
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (!string.IsNullOrEmpty(databaseUrl))
            {
                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');

                var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.AbsolutePath.TrimStart('/'),
                    SslMode = Npgsql.SslMode.Require,
                    TrustServerCertificate = true
                };

                builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionStringBuilder.ConnectionString;
            }

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // Add services to the container
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

            // Configure JWT authentication
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "your_super_secure_256bit_secret_key_here_12345678";

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });



            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();

            //// Read the PORT environment variable; fallback to 5000 if not set
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
            builder.WebHost.UseUrls($"http://*:{port}");


            var app = builder.Build();

            // Enable Swagger UI in non-production environments
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS middleware to allow any origin
            app.UseCors("AllowAnyOrigin");

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            // Enable authentication middleware
            app.UseAuthentication();

            // Enable authorization middleware
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();  // Ensures migrations are applied
            }
            // Run the app
            app.Run();
        }
    }
}

