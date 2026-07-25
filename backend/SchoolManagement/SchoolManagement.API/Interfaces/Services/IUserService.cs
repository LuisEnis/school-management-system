using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Enums;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<IEnumerable<UserDto>> GetByRoleAsync(UserRole role);

        Task<UserDetailsDto?> GetByIdAsync(int id);

        Task<UserDto> CreateAsync(CreateUserDto dto, UserRole currentUserRole);

        Task<bool> UpdateAsync(int id, UpdateUserDto dto, UserRole currentUserRole);

        Task<bool> DeleteAsync(int id, UserRole currentUserRole);

        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
