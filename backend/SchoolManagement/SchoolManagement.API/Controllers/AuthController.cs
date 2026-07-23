using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Services;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// Used to login.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var response =
                await _authService
                    .LoginAsync(dto);

            return Ok(response);
        }

        /// <summary>
        /// Used to logout.
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService
                .LogoutAsync();

            return Ok(new
            {
                message = "Logged out successfully."
            });
        }

        /// <summary>
        /// Returns the currently authenticated user.
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDetailsDto>> GetCurrentUser()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid authentication token.");
            }

            var user =
                await _userService.GetByIdAsync(userId);

            return Ok(user);
        }
    }
}
