using AutoMapper;
using SchoolManagement.API.DTOs.SchoolClasses;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Repositories;

namespace SchoolManagement.API.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public SchoolClassService(
            ISchoolClassRepository schoolClassRepository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _schoolClassRepository = schoolClassRepository;
            _assignmentRepository = assignmentRepository;
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
            var nameExists =
                await _schoolClassRepository
                    .NameExistsAsync(dto.Name);

            if (nameExists)
            {
                throw new ConflictException(
                    "A class with this name already exists.");
            }

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

            var nameExists =
                await _schoolClassRepository
                    .NameExistsAsync(dto.Name, id);

            if (nameExists)
            {
                throw new ConflictException(
                    "A class with this name already exists.");
            }

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

            var hasStudents =
                await _assignmentRepository
                    .ClassHasStudentsAsync(id);

            var hasAssignments =
                await _assignmentRepository
                    .TeacherHasTeachingAssignmentsAsync(id);


            if (hasStudents || hasAssignments)
            {
                throw new ConflictException(
                    "Cannot delete class because it has active assignments.");
            }

            _schoolClassRepository.Delete(schoolClass);

            await _schoolClassRepository.SaveChangesAsync();

            return true;
        }

        public async Task<ClassDetailsDto?> GetClassDetailsAsync(int classId, int? teacherId = null)
        {
            if (teacherId.HasValue)
            {
                var hasAccess =
                    await _assignmentRepository
                        .TeacherHasClassAsync(
                            teacherId.Value,
                            classId);

                if (!hasAccess)
                {
                    throw new ForbiddenException(
                        "You are not assigned to this class.");
                }
            }


            var schoolClass =
                await _schoolClassRepository
                    .GetClassDetailsAsync(classId);


            if (schoolClass == null)
                return null;


            return new ClassDetailsDto
            {
                Id = schoolClass.Id,

                Name = schoolClass.Name,

                Students = _mapper.Map<List<UserDto>>(
                    schoolClass.StudentClasses
                        .Select(sc => sc.Student)
                        .ToList()),

                Subjects =
                    schoolClass.TeachingAssignments
                    .Select(ta => new StudentSubjectDto
                    {
                        SubjectName = ta.Subject.Name,

                        TeacherName =
                            $"{ta.Teacher.FirstName} {ta.Teacher.LastName}"
                    })
                    .ToList()
            };
        }
    }
}
