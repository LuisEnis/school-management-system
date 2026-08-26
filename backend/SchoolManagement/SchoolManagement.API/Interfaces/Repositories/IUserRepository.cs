using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetAllAsync(UserQueryRequest request);

        Task<PagedResult<User>> GetByRoleAsync(UserRole role, UserQueryRequest request);

        Task<IEnumerable<User>> GetAllByRoleAsync(UserRole role);

        Task<int> CountByRoleAsync(UserRole role);

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);

        void Update(User user);

        void Delete(User user);

        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);

        Task SaveChangesAsync();
    }
}
