using SchoolManagement.API.DTOs.Students;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IStudentService
    {
        Task<StudentDashboardDto?> GetDashboardAsync(int studentId);
    }
}
