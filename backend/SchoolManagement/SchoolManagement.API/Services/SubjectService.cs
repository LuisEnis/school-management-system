using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.DTOs.Subjects;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Hubs;
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
        private readonly ILogger<SubjectService> _logger;
        private readonly IHubContext<SchoolHub> _hubContext;

        public SubjectService(
            ISubjectRepository subjectRepository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper,
            ILogger<SubjectService> logger,
            IHubContext<SchoolHub> hubContext)
        {
            _subjectRepository = subjectRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<PagedResult<SubjectDto>> GetAllAsync(SubjectQueryRequest request)
        {
            var result = await _subjectRepository.GetAllAsync(request);

            return new PagedResult<SubjectDto>
            {
                Items = _mapper.Map<IEnumerable<SubjectDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<IEnumerable<SubjectDto>> GetAllUnpagedAsync()
        {
            var subjects = await _subjectRepository.GetAllUnpagedAsync();

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

            _logger.LogInformation("Subject {SubjectId} ({SubjectName}) was created.", subject.Id, subject.Name);

            var subjectDto = _mapper.Map<SubjectDto>(subject);

            await _hubContext.Clients.All.SendAsync(
                "SubjectCreated",
                subjectDto);

            return subjectDto;
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

            _logger.LogInformation("Subject {SubjectId} ({SubjectName}) was updated.", subject.Id, subject.Name);

            var subjectDto = _mapper.Map<SubjectDto>(subject);

            await _hubContext.Clients.All.SendAsync(
                "SubjectUpdated",
                subjectDto);

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
                    .SubjectHasTeachingAssignmentsAsync(id);


            if (hasTeachers || hasClasses)
            {
                throw new ConflictException(
                    "Cannot delete subject because it has active assignments.");
            }

            _subjectRepository.Delete(subject);

            await _subjectRepository.SaveChangesAsync();

            _logger.LogInformation("Subject {SubjectId} ({SubjectName}) was deleted.", subject.Id, subject.Name);

            await _hubContext.Clients.All.SendAsync(
                "SubjectDeleted",
                subject.Id);

            return true;
        }
    }
}
