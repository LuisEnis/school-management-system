using SchoolManagement.API.DTOs.Auth;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);

        Task LogoutAsync();
    }
}
