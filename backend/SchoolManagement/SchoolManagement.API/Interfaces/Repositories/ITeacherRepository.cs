using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<TeachingAssignment>> GetTeacherClassesAsync(int teacherId);

        Task<IEnumerable<User>> GetStudentsByClassAsync(int classId);

    }
}
