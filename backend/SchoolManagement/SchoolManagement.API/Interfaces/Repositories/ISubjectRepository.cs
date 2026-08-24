using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ISubjectRepository
    {
        Task<PagedResult<Subject>> GetAllAsync(PaginationRequest request);

        Task<int> CountAsync();

        Task<IEnumerable<Subject>> GetAllUnpagedAsync();

        Task<Subject?> GetByIdAsync(int id);

        Task AddAsync(Subject subject);

        void Update(Subject subject);

        void Delete(Subject subject);

        Task<bool> NameExistsAsync(string name, int? excludeSubjectId = null);

        Task SaveChangesAsync();
    }
}
