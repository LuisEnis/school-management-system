using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            using var scope =
                services.CreateScope();

            var context =
                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            var passwordHasher =
                scope.ServiceProvider
                    .GetRequiredService<IPasswordHasherService>();

            var configuration =
                scope.ServiceProvider
                    .GetRequiredService<IConfiguration>();


            // Apply pending migrations
            await context.Database.MigrateAsync();


            // Check whether a Director already exists
            var directorExists =
                await context.Users
                    .AnyAsync(
                        u => u.Role == UserRole.Director);


            if (directorExists)
                return;


            // Read default Director configuration
            var directorSection =
                configuration.GetSection("DefaultDirector");


            // Create default Director
            var director = new User
            {
                Username =
                    directorSection["Username"]!,

                FirstName =
                    directorSection["FirstName"]!,

                LastName =
                    directorSection["LastName"]!,

                Email =
                    directorSection["Email"]!,

                Role = UserRole.Director
            };


            director.PasswordHash =
                passwordHasher.HashPassword(
                    director,
                    directorSection["Password"]!);


            context.Users.Add(director);

            await context.SaveChangesAsync();
        }
    }
}
