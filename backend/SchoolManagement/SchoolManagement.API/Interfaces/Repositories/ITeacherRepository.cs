using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<TeacherAssignmentDto>> GetTeacherClassesAsync(int teacherId);

        Task<IEnumerable<UserDto>> GetStudentsByClassAsync(int classId);

    }
}
