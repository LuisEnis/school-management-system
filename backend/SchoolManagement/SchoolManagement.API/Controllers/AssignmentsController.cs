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

        /// <summary>
        /// Assigns a student to a class using their ids.
        /// </summary>
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

        /// <summary>
        /// Assigns a teacher to a subject using their ids.
        /// </summary>
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

        /// <summary>
        /// Creates a connection between a teacher that teaches a certain subject and the class where he will teach it using their ids.
        /// </summary>
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


        /// <summary>
        /// Deletes the connection between a student and a class.
        /// </summary>
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


        /// <summary>
        /// Deletes the connection between a teacher and a subject.
        /// </summary>
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


        /// <summary>
        /// Deletes the connection between a teacher that teaches a certain subject and the class where he teaches it.
        /// </summary>
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
