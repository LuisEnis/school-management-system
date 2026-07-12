using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Enums;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<IEnumerable<UserDto>> GetByRoleAsync(UserRole role);

        Task<UserDetailsDto?> GetByIdAsync(int id);

        Task<UserDto> CreateAsync(CreateUserDto dto);

        Task<bool> UpdateAsync(int id, UpdateUserDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
