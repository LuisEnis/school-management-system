using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<StudentClass?> GetStudentClassAsync(int studentId);
    }
}
