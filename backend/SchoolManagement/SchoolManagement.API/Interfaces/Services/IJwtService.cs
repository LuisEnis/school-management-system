using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IJwtService
    {
        JwtTokenResultDto GenerateToken(User user);
    }
}
