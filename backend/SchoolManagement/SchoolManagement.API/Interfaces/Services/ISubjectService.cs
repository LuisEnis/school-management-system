using SchoolManagement.API.DTOs.Subjects;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllAsync();

        Task<SubjectDto?> GetByIdAsync(int id);

        Task<SubjectDto> CreateAsync(CreateSubjectDto dto);

        Task<bool> UpdateAsync(int id, UpdateSubjectDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
