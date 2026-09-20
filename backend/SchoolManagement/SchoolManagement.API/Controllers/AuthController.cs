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
            var result = await _authService.LoginAsync(dto);

            SetRefreshTokenCookie(
                result.RefreshToken,
                result.RefreshTokenExpiration);


            return Ok(
                new LoginResponseDto
                {
                    Token = result.Token,

                    Expiration = result.Expiration,

                    User = result.User
                });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> Refresh()
        {
            var refreshToken =
                Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                DeleteRefreshTokenCookie();

                throw new UnauthorizedException(
                    "Refresh token is missing.");
            }


            try
            {
                var result =
                    await _authService.RefreshAsync(
                        refreshToken);


                SetRefreshTokenCookie(
                    result.RefreshToken,
                    result.RefreshTokenExpiration);


                return Ok(
                    new LoginResponseDto
                    {
                        Token = result.Token,
                        Expiration = result.Expiration,
                        User = result.User
                    });
            }
            catch (UnauthorizedException)
            {
                DeleteRefreshTokenCookie();

                throw;
            }
        }

        private void SetRefreshTokenCookie(string refreshToken, DateTime expiration)
        {
            Response.Cookies.Append(
                "refreshToken",
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = expiration
                });
        }


        private void DeleteRefreshTokenCookie()
        {
            Response.Cookies.Delete(
                "refreshToken",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }

        /// <summary>
        /// Used to logout.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken =
                Request.Cookies["refreshToken"];


            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await _authService.LogoutAsync(
                    refreshToken);
            }


            DeleteRefreshTokenCookie();


            return Ok(
                new
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
