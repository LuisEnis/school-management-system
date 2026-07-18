using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IPasswordHasherService
    {
        string HashPassword(User user, string password);

        bool VerifyPassword(User user, string hashedPassword, string providedPassword);
    }
}
