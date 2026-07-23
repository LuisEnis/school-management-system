using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Interfaces.Services;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "TeacherOnly")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;


        public TeacherController(
            ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// Show the classes where this teacher teaches and which subject the teacher teaches in that class.
        /// </summary>
        [HttpGet("classes")]
        public async Task<ActionResult<IEnumerable<TeacherAssignmentDto>>> GetClasses()
        {
            var teacherId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier)!.Value);


            var result =
                await _teacherService
                    .GetClassesAsync(teacherId);


            return Ok(result);
        }

        /// <summary>
        /// Shows the students in a class where the teacher teaches.
        /// </summary>
        [HttpGet("classes/{classId}/students")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetStudentsByClass(int classId)
        {
            var teacherId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier)!.Value);


            var result =
                await _teacherService
                    .GetStudentsByClassAsync(
                        teacherId,
                        classId);


            return Ok(result);
        }
    }
}
