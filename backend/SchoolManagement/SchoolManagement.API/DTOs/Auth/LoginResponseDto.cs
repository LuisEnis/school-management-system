using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public UserDto User { get; set; } = null!;
    }
}
