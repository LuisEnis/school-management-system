using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SchoolManagement.API.Caching;
using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.DTOs.SchoolClasses;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Hubs;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class SchoolClassService : ISchoolClassService
    {
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SchoolClassService> _logger;
        private readonly IHubContext<SchoolHub> _hubContext;
        private readonly ICacheService _cacheService;

        public SchoolClassService(
            ISchoolClassRepository schoolClassRepository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper,
            ILogger<SchoolClassService> logger,
            IHubContext<SchoolHub> hubContext,
            ICacheService cacheService)
        {
            _schoolClassRepository = schoolClassRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _logger = logger;
            _hubContext = hubContext;
            _cacheService = cacheService;
        }

        public async Task<PagedResult<SchoolClassDto>> GetAllAsync(SchoolClassQueryRequest request)
        {
            var result =
                await _schoolClassRepository.GetAllAsync(request);

            return new PagedResult<SchoolClassDto>
            {
                Items = _mapper.Map<IEnumerable<SchoolClassDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<IEnumerable<SchoolClassDto>> GetAllUnpagedAsync()
        {
            var classes = await _schoolClassRepository.GetAllUnpagedAsync();

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

            _cacheService.Remove(CacheKeys.ManagementDashboard);

            _logger.LogInformation("School class {ClassId} ({ClassName}) was created.", schoolClass.Id, schoolClass.Name);

            var classDto = _mapper.Map<SchoolClassDto>(schoolClass);

            await _hubContext.Clients.All.SendAsync(
                "ClassCreated",
                classDto);

            return classDto;
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

            _logger.LogInformation("School class {ClassId} ({ClassName}) was updated.", schoolClass.Id, schoolClass.Name);

            var classDto = _mapper.Map<SchoolClassDto>(schoolClass);

            await _hubContext.Clients.All.SendAsync(
                "ClassUpdated",
                classDto);

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
                    .ClassHasTeachingAssignmentsAsync(id);


            if (hasStudents || hasAssignments)
            {
                throw new ConflictException(
                    "Cannot delete class because it has active assignments.");
            }

            _schoolClassRepository.Delete(schoolClass);

            await _schoolClassRepository.SaveChangesAsync();

            _cacheService.Remove(CacheKeys.ManagementDashboard);

            _logger.LogInformation("School class {ClassId} ({ClassName}) was deleted.", schoolClass.Id, schoolClass.Name);

            await _hubContext.Clients.All.SendAsync(
                "ClassDeleted",
                schoolClass.Id);

            return true;
        }

        public async Task<ClassDetailsDto?> GetClassDetailsAsync(int classId)
        {
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

        public async Task<ClassDetailsDto?> GetTeacherClassDetailsAsync(int classId, int teacherId)
        {

            var teacherHasClass =
                await _assignmentRepository
                    .TeacherHasClassAsync(
                        teacherId,
                        classId);


            if (!teacherHasClass)
            {
                throw new ForbiddenException(
                    "You are not assigned to this class.");
            }


            return await GetClassDetailsAsync(classId);

        }
    }
}
