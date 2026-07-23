using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherAssignmentDto>> GetClassesAsync(int teacherId);

        Task<IEnumerable<UserDto>> GetStudentsByClassAsync(int teacherId, int classId);
    }
}
