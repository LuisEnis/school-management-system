using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Students;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Services;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "StudentOnly")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;


        public StudentController(
            IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Returns what a students should see in its dashboard, like the class he is enrolled and the subjects he has and which teacher teaches them.
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<StudentDashboardDto>> GetDashboard()
        {
            var studentId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier)!.Value);


            var result =
                await _studentService
                    .GetDashboardAsync(studentId);


            if (result == null)
                return NotFound();


            return Ok(result);
        }
    }
}
