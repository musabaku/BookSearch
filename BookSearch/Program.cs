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

// using BookSearch.Data;
// using BookSearch.Services;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;

// namespace BookSearch
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             var builder = WebApplication.CreateBuilder(args);

//             // Configure CORS to allow any origin, method, and header
//             builder.Services.AddCors(options =>
//             {
//                 options.AddPolicy("AllowAnyOrigin", policy =>
//                 {
//                     policy.AllowAnyOrigin()        // Allow any origin
//                           .AllowAnyMethod()        // Allow any HTTP method
//                           .AllowAnyHeader();       // Allow any headers
//                 });
//             });

//             // Configure database connection
//             var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
//             if (!string.IsNullOrEmpty(databaseUrl))
//             {
//                 var databaseUri = new Uri(databaseUrl);
//                 var userInfo = databaseUri.UserInfo.Split(':');

//                 var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder
//                 {
//                     Host = databaseUri.Host,
//                     Port = databaseUri.Port,
//                     Username = userInfo[0],
//                     Password = userInfo[1],
//                     Database = databaseUri.AbsolutePath.TrimStart('/'),
//                     SslMode = Npgsql.SslMode.Require,
//                     TrustServerCertificate = true
//                 };

//                 builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionStringBuilder.ConnectionString;
//             }

//             builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                 options.UseNpgsql(
//                     builder.Configuration.GetConnectionString("DefaultConnection")
//                 ));

//             // Add services to the container
//             builder.Services.AddScoped<IBookServices, BookServices>();
//             builder.Services.AddScoped<IAuthService, AuthService>();
//             builder.Services.AddScoped<IFavouriteService, FavouriteService>();
//             builder.Services.AddLogging(config =>
//             {
//                 config.AddConsole();
//             });

//             builder.Logging.ClearProviders();
//             builder.Logging.AddConsole();
//             builder.Logging.SetMinimumLevel(LogLevel.Information);

//             // Configure JWT authentication
//             var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "your_super_secure_256bit_secret_key_here_12345678";

//             builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                 .AddJwtBearer(options =>
//                 {
//                     options.TokenValidationParameters = new TokenValidationParameters
//                     {
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
//                         ValidateLifetime = true,
//                         ValidateIssuer = false,
//                         ValidateAudience = false,
//                     };
//                 });



//             // Configure Swagger/OpenAPI
//             builder.Services.AddEndpointsApiExplorer();
//             builder.Services.AddSwaggerGen(options =>
//             {
//                 options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                 {
//                     Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
//                     Name = "Authorization",
//                     In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//                     Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
//                 });

//                 options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//                 {
//                     {
//                         new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                         {
//                             Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                             {
//                                 Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                                 Id = "Bearer"
//                             }
//                         },
//                         new string[] {}
//                     }
//                 });
//             });

//             builder.Services.AddControllers();
//             builder.Services.AddHttpClient();

//             //// Read the PORT environment variable; fallback to 5000 if not set
//             var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
//             builder.WebHost.UseUrls($"http://*:{port}");


//             var app = builder.Build();

//             // Enable Swagger UI in non-production environments
//             if (!app.Environment.IsProduction())
//             {
//                 app.UseSwagger();
//                 app.UseSwaggerUI();
//             }

//             // Enable CORS middleware to allow any origin
//             app.UseCors("AllowAnyOrigin");

//             // Enable HTTPS redirection
//             app.UseHttpsRedirection();

//             // Enable authentication middleware
//             app.UseAuthentication();

//             // Enable authorization middleware
//             app.UseAuthorization();

//             // Map controllers
//             app.MapControllers();

//             using (var scope = app.Services.CreateScope())
//             {
//                 var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//                 dbContext.Database.Migrate();  // Ensures migrations are applied
//             }
//             // Run the app
//             app.Run();
//         }
//     }
// }
using MealDiary.Data;
using MealDiary.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace MealDiary
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Read environment variables for database connection
                var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
                var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
                var dbUser = Environment.GetEnvironmentVariable("DB_USER");
                var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
                var dbName = Environment.GetEnvironmentVariable("DB_NAME");

                if (!string.IsNullOrEmpty(dbHost) && !string.IsNullOrEmpty(dbUser) && 
                    !string.IsNullOrEmpty(dbPass) && !string.IsNullOrEmpty(dbName))
                {
                    var connectionString = new NpgsqlConnectionStringBuilder
                    {
                        Host = dbHost,
                        Port = string.IsNullOrEmpty(dbPort) ? 5432 : int.Parse(dbPort),
                        Username = dbUser,
                        Password = dbPass,
                        Database = dbName,
                        SslMode = SslMode.Prefer, // Changed from Require to Prefer for better compatibility
                        TrustServerCertificate = false // Ensures SSL validation
                    }.ConnectionString;

                    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
                    Console.WriteLine($"Database connection string configured. Connecting to {dbHost}:{dbPort}");
                }
                else
                {
                    Console.WriteLine("Error: One or more database environment variables are missing.");
                    return; // Stop execution if DB config is invalid
                }

                // Test database connection before proceeding
                try
                {
                    using (var testConnection = new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")))
                    {
                        await testConnection.OpenAsync();
                        Console.WriteLine("✅ Database connection successful.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Database connection failed: {ex.Message}");
                    return; // Stop execution if DB is not reachable
                }

                // Configure DbContext
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

                    if (builder.Environment.IsDevelopment())
                    {
                        options.EnableSensitiveDataLogging();
                        options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
                    }
                });

                // JWT Authentication Configuration
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false; // Change to true in production
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                               builder.Configuration["AppSettings:JwtSecretKey"] ?? "default_secret_key")),
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           ClockSkew = TimeSpan.Zero
                       };
                   });

                // Swagger setup with JWT authentication
                builder.Services.AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                        Name = "Authorization",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

                // Register services
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddScoped<IUserAuthService, UserAuthService>();
                builder.Services.AddScoped<ITagService, TagService>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();
                builder.Services.AddScoped<IRecipeService, RecipeService>();

                // Enable CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: "AllowAllOrigins",
                        policy =>
                        {
                            policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
                });

                var app = builder.Build();

                // Middleware Setup
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Conditionally enable HTTPS redirection (disable in Docker)
                if (!app.Environment.IsDevelopment())
                {
                    app.UseHttpsRedirection();
                }

                app.UseAuthentication();
                app.UseCors("AllowAllOrigins");
                app.UseAuthorization();

                

                app.MapControllers();

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fatal error during application startup: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}

