using AutoMapper;
using SchoolManagement.API.DTOs.Subjects;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Repositories;

namespace SchoolManagement.API.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public SubjectService(
            ISubjectRepository subjectRepository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);

            if (subject == null)
                return null;

            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> CreateAsync(CreateSubjectDto dto)
        {
            var nameExists =
                await _subjectRepository
                    .NameExistsAsync(dto.Name);

            if (nameExists)
            {
                throw new ConflictException(
                    "A subject with this name already exists.");
            }

            var subject = _mapper.Map<Subject>(dto);

            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();

            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<bool> UpdateAsync(int id, UpdateSubjectDto dto)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);

            if (subject == null)
                return false;

            var nameExists =
                await _subjectRepository
                    .NameExistsAsync(dto.Name, id);

            if (nameExists)
            {
                throw new ConflictException(
                    "A subject with this name already exists.");
            }

            _mapper.Map(dto, subject);

            _subjectRepository.Update(subject);

            await _subjectRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);

            if (subject == null)
                return false;

            var hasTeachers =
                await _assignmentRepository
                    .SubjectHasTeacherAssignmentsAsync(id);


            var hasClasses =
                await _assignmentRepository
                    .TeacherHasTeachingAssignmentsAsync(id);


            if (hasTeachers || hasClasses)
            {
                throw new ConflictException(
                    "Cannot delete subject because it has active assignments.");
            }

            _subjectRepository.Delete(subject);

            await _subjectRepository.SaveChangesAsync();

            return true;
        }
    }
}
