using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.SchoolClasses;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolClassesController : ControllerBase
    {
        private readonly ISchoolClassService _schoolClassService;

        public SchoolClassesController(
            ISchoolClassService schoolClassService)
        {
            _schoolClassService = schoolClassService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolClassDto>>> GetAll()
        {
            var classes = await _schoolClassService.GetAllAsync();

            return Ok(classes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolClassDto>> GetById(int id)
        {
            var schoolClass = await _schoolClassService.GetByIdAsync(id);

            if (schoolClass == null)
                return NotFound();

            return Ok(schoolClass);
        }

        [HttpPost]
        public async Task<ActionResult<SchoolClassDto>> Create(CreateSchoolClassDto dto)
        {
            var schoolClass = await _schoolClassService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = schoolClass.Id },
                schoolClass);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSchoolClassDto dto)
        {
            var result = await _schoolClassService.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _schoolClassService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
