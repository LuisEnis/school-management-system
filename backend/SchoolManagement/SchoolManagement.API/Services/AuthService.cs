using AutoMapper;
using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;


        public AuthService(
            IUserRepository userRepository,
            IPasswordHasherService passwordHasherService,
            IJwtService jwtService,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _jwtService = jwtService;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
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


            return new LoginResponseDto
            {
                Token = jwt.Token,

                Expiration = jwt.Expiration,

                User =
                    _mapper.Map<UserDto>(user)
            };
        }


        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
