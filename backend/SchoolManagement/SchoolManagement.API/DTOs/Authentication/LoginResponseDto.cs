using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.DTOs.Authentication
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public UserDto User { get; set; }
    }
}
