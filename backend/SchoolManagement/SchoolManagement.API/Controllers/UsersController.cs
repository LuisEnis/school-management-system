using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "DirectorOnly")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Director")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetStudents()
        {
            var users = await _userService.GetByRoleAsync(UserRole.Student);

            return Ok(users);
        }

        [HttpGet("teachers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetTeachers()
        {
            var users = await _userService.GetByRoleAsync(UserRole.Teacher);

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var user = await _userService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var result = await _userService.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
