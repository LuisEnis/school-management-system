using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllAsync();

        Task<Subject?> GetByIdAsync(int id);

        Task AddAsync(Subject subject);

        void Update(Subject subject);

        void Delete(Subject subject);

        Task<bool> NameExistsAsync(string name, int? excludeSubjectId = null);

        Task SaveChangesAsync();
    }
}
