using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.Subjects;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Management")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Retrieves all subjects.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();

            return Ok(subjects);
        }

        /// <summary>
        /// Retrieves subject by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);

            if (subject == null)
                return NotFound();

            return Ok(subject);
        }

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> Create(CreateSubjectDto dto)
        {
            var subject = await _subjectService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = subject.Id },
                subject);
        }

        /// <summary>
        /// Updates an existing subject.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSubjectDto dto)
        {
            var result = await _subjectService.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subjectService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
