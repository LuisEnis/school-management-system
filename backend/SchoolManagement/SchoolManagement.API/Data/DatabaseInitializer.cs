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


            // Apply pending migrations
            await context.Database.MigrateAsync();


            // Check whether a Director already exists
            var directorExists =
                await context.Users
                    .AnyAsync(
                        u => u.Role == UserRole.Director);


            if (directorExists)
                return;


            // Create default Director
            var director = new User
            {
                Username = "admin",
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@schoolmanagement.com",
                Role = UserRole.Director
            };


            director.PasswordHash =
                passwordHasher.HashPassword(
                    director,
                    "admin123");


            context.Users.Add(director);

            await context.SaveChangesAsync();
        }
    }
}
