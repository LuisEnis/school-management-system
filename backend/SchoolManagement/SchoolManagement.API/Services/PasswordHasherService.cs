using Microsoft.AspNetCore.Identity;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordHasherService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                hashedPassword,
                providedPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
