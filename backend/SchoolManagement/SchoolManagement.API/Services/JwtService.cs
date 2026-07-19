using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.API.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }


        public JwtTokenResultDto GenerateToken(User user)
        {
            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.Key));


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role.ToString())
            };

            var expiration =
                DateTime.UtcNow.AddMinutes(
                        _jwtSettings.ExpiryMinutes);

            var jwtToken =
                new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credentials);


            return new JwtTokenResultDto
            {
                Token =
                    new JwtSecurityTokenHandler()
                        .WriteToken(jwtToken),

                Expiration = expiration
            };
        }
    }
}
