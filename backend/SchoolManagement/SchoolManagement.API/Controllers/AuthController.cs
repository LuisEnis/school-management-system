using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Auth;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var response =
                await _authService
                    .LoginAsync(dto);

            return Ok(response);
        }


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
    }
}
