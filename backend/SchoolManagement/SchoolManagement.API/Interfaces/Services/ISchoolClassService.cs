using SchoolManagement.API.DTOs.SchoolClasses;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface ISchoolClassService
    {
        Task<IEnumerable<SchoolClassDto>> GetAllAsync();

        Task<SchoolClassDto?> GetByIdAsync(int id);

        Task<SchoolClassDto> CreateAsync(CreateSchoolClassDto dto);

        Task<bool> UpdateAsync(int id, UpdateSchoolClassDto dto);

        Task<bool> DeleteAsync(int id);

        Task<ClassDetailsDto?> GetClassDetailsAsync(int classId);

        Task<ClassDetailsDto?> GetTeacherClassDetailsAsync(int classId, int teacherId);
    }
}
