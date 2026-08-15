using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs.ManagementDashboard;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Management")]
    public class ManagementDashboardController : ControllerBase
    {
        private readonly IManagementDashboardService _dashboardService;

        public ManagementDashboardController(
            IManagementDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Returns dashboard statistics for directors and secretaries.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ManagementDashboardDto>> GetDashboard()
        {
            var result =
                await _dashboardService
                    .GetDashboardAsync();

            return Ok(result);
        }
    }
}
