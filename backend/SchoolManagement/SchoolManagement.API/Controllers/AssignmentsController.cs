using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Assignments;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Management")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentsController(
            IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }


        [HttpPost("student-class")]
        public async Task<IActionResult> AssignStudentToClass(
            CreateStudentClassAssignmentDto dto)
        {
            var result =
                await _assignmentService
                    .AssignStudentToClassAsync(dto);

            return CreatedAtAction(
                nameof(AssignStudentToClass),
                result);
        }


        [HttpPost("teacher-subject")]
        public async Task<IActionResult> AssignTeacherToSubject(
            CreateTeacherSubjectAssignmentDto dto)
        {
            var result =
                await _assignmentService
                    .AssignTeacherToSubjectAsync(dto);

            return CreatedAtAction(
                nameof(AssignTeacherToSubject),
                result);
        }


        [HttpPost("teaching-assignment")]
        public async Task<IActionResult> AssignTeachingAssignment(
            CreateTeachingAssignmentDto dto)
        {
            var result =
                await _assignmentService
                    .AssignTeacherToClassSubjectAsync(dto);

            return CreatedAtAction(
                nameof(AssignTeachingAssignment),
                result);
        }



        [HttpDelete("student-class")]
        public async Task<IActionResult> RemoveStudentFromClass(
            int studentId,
            int schoolClassId)
        {
            var removed =
                await _assignmentService
                    .RemoveStudentFromClassAsync(
                        studentId,
                        schoolClassId);

            if (!removed)
                return NotFound();

            return NoContent();
        }



        [HttpDelete("teacher-subject")]
        public async Task<IActionResult> RemoveTeacherFromSubject(
            int teacherId,
            int subjectId)
        {
            var removed =
                await _assignmentService
                    .RemoveTeacherFromSubjectAsync(
                        teacherId,
                        subjectId);

            if (!removed)
                return NotFound();

            return NoContent();
        }



        [HttpDelete("teaching-assignment")]
        public async Task<IActionResult> RemoveTeachingAssignment(
            int schoolClassId,
            int subjectId,
            int teacherId)
        {
            var removed =
                await _assignmentService
                    .RemoveTeachingAssignmentAsync(
                        schoolClassId,
                        subjectId,
                        teacherId);

            if (!removed)
                return NotFound();

            return NoContent();
        }
    }
}
