using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);

        Task<User?> GetByIdAsync(int id);

        Task AddAsync(User user);

        void Update(User user);

        void Delete(User user);

        Task SaveChangesAsync();
    }
}
