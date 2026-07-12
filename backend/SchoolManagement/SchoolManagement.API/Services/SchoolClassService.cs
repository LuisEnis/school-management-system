using AutoMapper;
using SchoolManagement.API.DTOs.SchoolClasses;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IMapper _mapper;

        public SchoolClassService(
            ISchoolClassRepository schoolClassRepository,
            IMapper mapper)
        {
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SchoolClassDto>> GetAllAsync()
        {
            var classes = await _schoolClassRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<SchoolClassDto>>(classes);
        }

        public async Task<SchoolClassDto?> GetByIdAsync(int id)
        {
            var schoolClass = await _schoolClassRepository.GetByIdAsync(id);

            if (schoolClass == null)
                return null;

            return _mapper.Map<SchoolClassDto>(schoolClass);
        }

        public async Task<SchoolClassDto> CreateAsync(CreateSchoolClassDto dto)
        {
            var schoolClass = _mapper.Map<SchoolClass>(dto);

            await _schoolClassRepository.AddAsync(schoolClass);
            await _schoolClassRepository.SaveChangesAsync();

            return _mapper.Map<SchoolClassDto>(schoolClass);
        }

        public async Task<bool> UpdateAsync(int id, UpdateSchoolClassDto dto)
        {
            var schoolClass = await _schoolClassRepository.GetByIdAsync(id);

            if (schoolClass == null)
                return false;

            _mapper.Map(dto, schoolClass);

            _schoolClassRepository.Update(schoolClass);

            await _schoolClassRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schoolClass = await _schoolClassRepository.GetByIdAsync(id);

            if (schoolClass == null)
                return false;

            _schoolClassRepository.Delete(schoolClass);

            await _schoolClassRepository.SaveChangesAsync();

            return true;
        }
    }
}
