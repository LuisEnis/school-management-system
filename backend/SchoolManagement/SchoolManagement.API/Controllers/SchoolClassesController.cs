using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.SchoolClasses;
using SchoolManagement.API.Interfaces.Services;
using System.Security.Claims;

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

        /// <summary>
        /// Gets all the classes.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<IEnumerable<SchoolClassDto>>> GetAll()
        {
            var classes = await _schoolClassService.GetAllAsync();

            return Ok(classes);
        }

        /// <summary>
        /// Gets a class through its id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<SchoolClassDto>> GetById(int id)
        {
            var schoolClass = await _schoolClassService.GetByIdAsync(id);

            if (schoolClass == null)
                return NotFound();

            return Ok(schoolClass);
        }

        /// <summary>
        /// Creates a new class.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<SchoolClassDto>> Create(CreateSchoolClassDto dto)
        {
            var schoolClass = await _schoolClassService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = schoolClass.Id },
                schoolClass);
        }

        /// <summary>
        /// Updates an existing class.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "Management")]
        public async Task<IActionResult> Update(int id, UpdateSchoolClassDto dto)
        {
            var result = await _schoolClassService.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a class.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "Management")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _schoolClassService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Gets the class details.
        /// </summary>
        [HttpGet("{id}/details")]
        [Authorize(Policy = "Management")]
        public async Task<ActionResult<ClassDetailsDto>> GetDetails(int id)
        {
            var result =
                await _schoolClassService
                    .GetClassDetailsAsync(id);


            if (result == null)
                return NotFound();


            return Ok(result);
        }
    }
}
