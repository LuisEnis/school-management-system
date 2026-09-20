using AutoMapper;
using Microsoft.Extensions.Options;
using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Settings;

namespace SchoolManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasherService passwordHasherService,
            IJwtService jwtService,
            IMapper mapper,
            ILogger<AuthService> logger,
            IRefreshTokenService refreshTokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _jwtService = jwtService;
            _mapper = mapper;
            _logger = logger;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtOptions.Value;
        }


        public async Task<AuthResultDto> LoginAsync(LoginRequestDto dto)
        {
            var user =
                await _userRepository
                    .GetByEmailAsync(dto.Email);


            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for email {Email}.", dto.Email);

                throw new UnauthorizedException(
                    "Invalid email or password.");
            }


            var passwordValid =
                _passwordHasherService
                    .VerifyPassword(
                        user,
                        user.PasswordHash,
                        dto.Password);


            if (!passwordValid)
            {
                _logger.LogWarning("Failed login attempt for email {Email}. Password is not valid.", dto.Email);
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }

            _logger.LogInformation("User {UserId} ({Email}) logged in successfully.", user.Id, user.Email);


            var jwt =
                _jwtService.GenerateToken(user);

            var refreshToken =
                _refreshTokenService.GenerateToken();

            var refreshTokenHash =
                _refreshTokenService.HashToken(refreshToken);

            var refreshTokenExpiration =
                DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

            var refreshTokenEntity =
                new RefreshToken
                {
                    TokenHash = refreshTokenHash,

                    CreatedAt = DateTime.UtcNow,

                    ExpiresAt = refreshTokenExpiration,

                    UserId = user.Id
                };

            await _refreshTokenRepository
                .AddAsync(refreshTokenEntity);

            await _refreshTokenRepository
                .SaveChangesAsync();


            return new AuthResultDto
            {
                Token = jwt.Token,

                Expiration = jwt.Expiration,

                RefreshToken = refreshToken,

                RefreshTokenExpiration = refreshTokenExpiration,

                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResultDto> RefreshAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedException("Refresh token is missing.");
            }

            var refreshTokenHash =
                _refreshTokenService.HashToken(refreshToken);

            var storedRefreshToken =
                await _refreshTokenRepository
                    .GetByTokenHashAsync(refreshTokenHash);

            if (storedRefreshToken == null)
            {
                _logger.LogWarning(
                    "Refresh attempt failed because the refresh token was not found.");

                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (!storedRefreshToken.IsActive)
            {
                _logger.LogWarning(
                    "Refresh attempt failed for user {UserId}. Refresh token is expired or revoked.",
                    storedRefreshToken.UserId);

                throw new UnauthorizedException(
                    "Refresh token is expired or revoked.");
            }


            // Generate the new access token.
            var jwt =
                _jwtService.GenerateToken(
                    storedRefreshToken.User);


            // Generate the replacement refresh token.
            var newRefreshToken =
                _refreshTokenService.GenerateToken();

            var newRefreshTokenHash =
                _refreshTokenService.HashToken(
                    newRefreshToken);

            var newRefreshTokenExpiration =
                DateTime.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpiryDays);


            // Revoke the old refresh token and connect it
            // to the replacement token.
            storedRefreshToken.RevokedAt =
                DateTime.UtcNow;

            storedRefreshToken.ReplacedByTokenHash =
                newRefreshTokenHash;


            var newRefreshTokenEntity =
                new RefreshToken
                {
                    TokenHash = newRefreshTokenHash,

                    CreatedAt = DateTime.UtcNow,

                    ExpiresAt = newRefreshTokenExpiration,

                    UserId = storedRefreshToken.UserId
                };


            _refreshTokenRepository.Update(
                storedRefreshToken);

            await _refreshTokenRepository.AddAsync(
                newRefreshTokenEntity);

            await _refreshTokenRepository
                .SaveChangesAsync();


            _logger.LogInformation(
                "Refresh token rotated successfully for user {UserId}.",
                storedRefreshToken.UserId);


            return new AuthResultDto
            {
                Token = jwt.Token,

                Expiration = jwt.Expiration,

                RefreshToken = newRefreshToken,

                RefreshTokenExpiration =
                    newRefreshTokenExpiration,

                User =
                    _mapper.Map<UserDto>(
                        storedRefreshToken.User)
            };
        }


        public async Task LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            var refreshTokenHash =
                _refreshTokenService.HashToken(refreshToken);

            var storedRefreshToken =
                await _refreshTokenRepository
                    .GetByTokenHashAsync(refreshTokenHash);


            if (storedRefreshToken == null)
            {
                return;
            }


            if (storedRefreshToken.IsActive)
            {
                storedRefreshToken.RevokedAt =
                    DateTime.UtcNow;

                _refreshTokenRepository.Update(
                    storedRefreshToken);

                await _refreshTokenRepository
                    .SaveChangesAsync();


                _logger.LogInformation(
                    "Refresh token revoked during logout for user {UserId}.",
                    storedRefreshToken.UserId);
            }
        }
    }
}
