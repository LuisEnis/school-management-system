using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.DTOs.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiration { get; set; }

        public UserDto User { get; set; } = null!;
    }
}
