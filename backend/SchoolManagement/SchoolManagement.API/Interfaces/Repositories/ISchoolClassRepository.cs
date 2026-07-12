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

        Task SaveChangesAsync();
    }
}
