using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface ISchoolClassRepository
    {
        Task<PagedResult<SchoolClass>> GetAllAsync(PaginationRequest request);

        Task<int> CountAsync();

        Task<IEnumerable<SchoolClass>> GetAllUnpagedAsync();

        Task<SchoolClass?> GetByIdAsync(int id);

        Task AddAsync(SchoolClass schoolClass);

        void Update(SchoolClass schoolClass);

        void Delete(SchoolClass schoolClass);

        Task<bool> NameExistsAsync(string name, int? excludeClassId = null);

        Task SaveChangesAsync();

        Task<SchoolClass?> GetClassDetailsAsync(int classId);
    }
}
