using Microsoft.IdentityModel.Tokens;
using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.API.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public JwtTokenResultDto GenerateToken(User user)
        {
            var jwtSettings =
                _configuration.GetSection("Jwt");


            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtSettings["Key"]!));


            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
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
                    double.Parse(
                        jwtSettings["ExpiryMinutes"]!));

            var jwtToken =
                new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
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
