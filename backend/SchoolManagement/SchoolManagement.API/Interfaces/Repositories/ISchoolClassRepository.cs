using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ISchoolClassRepository
    {
        Task<IEnumerable<SchoolClass>> GetAllAsync();

        Task<SchoolClass?> GetByIdAsync(int id);

        Task AddAsync(SchoolClass schoolClass);

        void Update(SchoolClass schoolClass);

        void Delete(SchoolClass schoolClass);

        Task<bool> NameExistsAsync(string name, int? excludeClassId = null);

        Task<bool> HasStudentsAsync(int classId);

        Task<bool> HasTeachingAssignmentsAsync(int classId);

        Task SaveChangesAsync();
    }
}
