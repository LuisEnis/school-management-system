using SchoolManagement.API.DTOs.Auth;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginRequestDto dto);

        Task<AuthResultDto> RefreshAsync(string refreshToken);

        Task LogoutAsync(string refreshToken);
    }
}
