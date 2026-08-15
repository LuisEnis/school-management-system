using SchoolManagement.API.DTOs.ManagementDashboard;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IManagementDashboardService
    {
        Task<ManagementDashboardDto> GetDashboardAsync();
    }
}
