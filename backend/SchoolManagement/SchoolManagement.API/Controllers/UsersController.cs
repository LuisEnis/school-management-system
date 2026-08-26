using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Services;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all the users.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAllUsers([FromQuery] UserQueryRequest request)
        {
            var users = await _userService.GetAllAsync(request);

            return Ok(users);
        }

        /// <summary>
        /// Gets a user by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<UserDetailsDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Gets all the students.
        /// </summary>
        [HttpGet("students")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetStudents([FromQuery] UserQueryRequest request)
        {
            var users = await _userService.GetByRoleAsync(UserRole.Student, request);

            return Ok(users);
        }

        /// <summary>
        /// Gets all the students.
        /// </summary>
        [HttpGet("students/all")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetStudentsUnpaged()
        {
            var users = await _userService.GetAllByRoleAsync(UserRole.Student);

            return Ok(users);
        }

        /// <summary>
        /// Gets all the teachers.
        /// </summary>
        [HttpGet("teachers")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetTeachers([FromQuery] UserQueryRequest request)
        {
            var users = await _userService.GetByRoleAsync(UserRole.Teacher, request);

            return Ok(users);
        }

        /// <summary>
        /// Gets all the teachers.
        /// </summary>
        [HttpGet("teachers/all")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetTeachersUnpaged()
        {
            var users = await _userService.GetAllByRoleAsync(UserRole.Teacher);

            return Ok(users);
        }

        /// <summary>
        /// Gets all the secretaries.
        /// </summary>
        [HttpGet("secretaries")]
        [Authorize(Policy = "DirectorOnly")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetSecretaries([FromQuery] UserQueryRequest request)
        {
            var users =
                await _userService.GetByRoleAsync(UserRole.Secretary, request);

            return Ok(users);
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var user = await _userService.CreateAsync(dto, GetCurrentUserRole());

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                user);
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "Management")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var result = await _userService.UpdateAsync(id, dto, GetCurrentUserRole());

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Management")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id, GetCurrentUserRole());

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get the role of the user currently logged in.
        /// </summary>
        private UserRole GetCurrentUserRole()
        {
            return Enum.Parse<UserRole>(
                User.FindFirst(ClaimTypes.Role)!.Value);
        }

        /// <summary>
        /// Changes the password of the currently authenticated user.
        /// </summary>
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedException("Invalid token.");

            var userId = int.Parse(userIdClaim.Value);

            await _userService.ChangePasswordAsync(userId, dto);

            return NoContent();
        }
    }
}
