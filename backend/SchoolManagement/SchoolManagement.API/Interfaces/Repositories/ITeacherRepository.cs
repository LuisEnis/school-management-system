using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<TeacherClassDto>> GetTeacherClassesAsync(int teacherId);

        Task<IEnumerable<UserDto>> GetStudentsByClassAsync(int classId);

        Task<bool> TeacherHasClassAsync(int teacherId, int classId);
    }
}
