using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
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


        [HttpGet("classes")]
        public async Task<ActionResult<IEnumerable<TeacherClassDto>>> GetClasses()
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
