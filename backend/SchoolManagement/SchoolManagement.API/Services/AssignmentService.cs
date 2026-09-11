using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SchoolManagement.API.DTOs.Assignments;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Hubs;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssignmentService> _logger;
        private readonly IHubContext<SchoolHub> _hubContext;


        public AssignmentService(IAssignmentRepository assignmentRepository, IUserRepository userRepository, ISubjectRepository subjectRepository, ISchoolClassRepository schoolClassRepository, IMapper mapper, ILogger<AssignmentService> logger, IHubContext<SchoolHub> hubContext)
        {
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
            _logger = logger;
            _hubContext = hubContext;
        }


        public async Task<StudentClassAssignmentDto> AssignStudentToClassAsync(CreateStudentClassAssignmentDto dto)
        {
            var student =
                await _userRepository
                    .GetByIdAsync(dto.StudentId);

            if (student == null)
            {
                throw new NotFoundException(
                    "Student not found.");
            }


            if (student.Role != UserRole.Student)
            {
                throw new BadRequestException(
                    "User is not a student.");
            }

            var schoolClass =
                await _schoolClassRepository
                    .GetByIdAsync(dto.SchoolClassId);

            if (schoolClass == null)
            {
                throw new NotFoundException(
                    "Class not found.");
            }

            var alreadyAssigned =
                await _assignmentRepository
                    .StudentAlreadyAssignedToClassAsync(
                        dto.StudentId);

            if (alreadyAssigned)
            {
                throw new BadRequestException(
                    "Student already belongs to a class.");
            }

            var entity = _mapper.Map<StudentClass>(dto);

            await _assignmentRepository
                .AddStudentClassAsync(entity);

            await _assignmentRepository
                .SaveChangesAsync();

            _logger.LogInformation("Student {StudentId} was assigned to class {ClassId}.", dto.StudentId, dto.SchoolClassId);

            var assignmentDto =
                _mapper.Map<StudentClassAssignmentDto>(entity);

            await _hubContext.Clients.All.SendAsync(
                "StudentClassAssigned",
                assignmentDto);

            return assignmentDto;
        }


        public async Task<TeacherSubjectAssignmentDto> AssignTeacherToSubjectAsync(CreateTeacherSubjectAssignmentDto dto)
        {
            var teacher =
                await _userRepository
                    .GetByIdAsync(dto.TeacherId);

            if (teacher == null)
            {
                throw new NotFoundException(
                    "Teacher not found.");
            }


            if (teacher.Role != UserRole.Teacher)
            {
                throw new BadRequestException(
                    "User is not a teacher.");
            }

            var subject =
                await _subjectRepository
                    .GetByIdAsync(dto.SubjectId);

            if (subject == null)
            {
                throw new NotFoundException(
                    "Subject not found.");
            }

            var exists =
                await _assignmentRepository
                    .GetTeacherSubjectAsync(
                        dto.TeacherId,
                        dto.SubjectId);


            if (exists != null)
            {
                throw new ConflictException(
                    "Teacher is already assigned to this subject.");
            }

            var entity = _mapper.Map<TeacherSubject>(dto);

            await _assignmentRepository
                .AddTeacherSubjectAsync(entity);

            await _assignmentRepository
                .SaveChangesAsync();

            _logger.LogInformation("Teacher {TeacherId} was assigned to subject {SubjectId}.", dto.TeacherId, dto.SubjectId);

            var teacherSubjectDto = _mapper.Map<TeacherSubjectAssignmentDto>(entity);

            await _hubContext.Clients.All.SendAsync(
                "TeacherSubjectAssigned",
                teacherSubjectDto);

            return teacherSubjectDto;
        }


        public async Task<TeachingAssignmentDto> AssignTeacherToClassSubjectAsync(CreateTeachingAssignmentDto dto)
        {
            var teacher =
                await _userRepository
                    .GetByIdAsync(dto.TeacherId);

            if (teacher == null)
            {
                throw new NotFoundException(
                    "Teacher not found.");
            }


            if (teacher.Role != UserRole.Teacher)
            {
                throw new BadRequestException(
                    "User is not a teacher.");
            }

            var subject =
                await _subjectRepository
                    .GetByIdAsync(dto.SubjectId);

            if (subject == null)
            {
                throw new NotFoundException(
                    "Subject not found.");
            }

            var schoolClass =
                await _schoolClassRepository
                    .GetByIdAsync(dto.SchoolClassId);

            if (schoolClass == null)
            {
                throw new NotFoundException(
                    "Class not found.");
            }

            var teacherCanTeach =
                await _assignmentRepository
                    .TeacherCanTeachSubjectAsync(
                        dto.TeacherId,
                        dto.SubjectId);


            if (!teacherCanTeach)
            {
                throw new BadRequestException(
                    "Teacher is not assigned to this subject.");
            }


            var subjectAlreadyAssigned =
                await _assignmentRepository
                    .TeachingAssignmentExistsForClassAsync(
                        dto.SchoolClassId,
                        dto.SubjectId);


            if (subjectAlreadyAssigned)
            {
                throw new ConflictException(
                    "This subject is already assigned to this class.");
            }


            var entity = _mapper.Map<TeachingAssignment>(dto);


            await _assignmentRepository
                .AddTeachingAssignmentAsync(entity);


            await _assignmentRepository
                .SaveChangesAsync();

            _logger.LogInformation("Teacher {TeacherId} was assigned to subject {SubjectId} in class {ClassId}.", dto.TeacherId, dto.SubjectId, dto.SchoolClassId);

            var teachingAssignmentDto = _mapper.Map<TeachingAssignmentDto>(entity);

            await _hubContext.Clients.All.SendAsync(
                "TeachingAssignmentCreated",
                teachingAssignmentDto);

            return teachingAssignmentDto;
        }


        public async Task<bool> RemoveStudentFromClassAsync(int studentId, int classId)
        {
            var assignment =
                await _assignmentRepository
                    .GetStudentClassAsync(studentId, classId);

            if (assignment == null)
                return false;

            _assignmentRepository.DeleteStudentClass(assignment);

            await _assignmentRepository.SaveChangesAsync();

            _logger.LogInformation("Student {studentId} was removed from class {classId}.", studentId, classId);

            await _hubContext.Clients.All.SendAsync(
                "StudentClassRemoved",
                studentId,
                classId);

            return true;
        }


        public async Task<bool> RemoveTeacherFromSubjectAsync(int teacherId, int subjectId)
        {
            var assignment =
                await _assignmentRepository
                    .GetTeacherSubjectAsync(
                        teacherId,
                        subjectId);


            if (assignment == null)
                return false;


            var isTeaching =
                await _assignmentRepository
                    .HasTeachingAssignmentAsync(
                        teacherId,
                        subjectId);


            if (isTeaching)
            {
                throw new ConflictException(
                    "Teacher cannot be removed from this subject because they are currently teaching it.");
            }


            _assignmentRepository.DeleteTeacherSubject(assignment);

            await _assignmentRepository.SaveChangesAsync();

            _logger.LogInformation("Teacher {teacherId} no longer teaches subject {subjectId}.", teacherId, subjectId);

            await _hubContext.Clients.All.SendAsync(
                "TeacherSubjectRemoved",
                teacherId,
                subjectId);

            return true;
        }


        public async Task<bool> RemoveTeachingAssignmentAsync(int classId, int subjectId, int teacherId)
        {
            var assignment =
                await _assignmentRepository
                    .GetTeachingAssignmentAsync(
                        classId,
                        subjectId,
                        teacherId);

            if (assignment == null)
                return false;

            _assignmentRepository.DeleteTeachingAssignment(assignment);

            await _assignmentRepository.SaveChangesAsync();

            _logger.LogInformation("Teacher {teacherId} no longer teaches subject {subjectId} in class {classId}.", teacherId, subjectId, classId);

            await _hubContext.Clients.All.SendAsync(
                "TeachingAssignmentRemoved",
                classId,
                subjectId,
                teacherId);

            return true;
        }

        public async Task<IEnumerable<StudentClassAssignmentDto>> GetStudentClassAssignmentsAsync()
        {
            var assignments =
                await _assignmentRepository
                    .GetStudentClassAssignmentsAsync();


            return assignments.Select(x => new StudentClassAssignmentDto
            {
                StudentId = x.StudentId,

                StudentName =
                    $"{x.Student.FirstName} {x.Student.LastName}",

                SchoolClassId = x.SchoolClassId,

                SchoolClassName =
                    x.SchoolClass.Name
            });
        }

        public async Task<IEnumerable<TeacherSubjectAssignmentDto>> GetTeacherSubjectAssignmentsAsync()
        {
            var assignments =
                await _assignmentRepository
                    .GetTeacherSubjectAssignmentsAsync();


            return assignments.Select(x => new TeacherSubjectAssignmentDto
            {
                TeacherId = x.TeacherId,

                TeacherName =
                    $"{x.Teacher.FirstName} {x.Teacher.LastName}",

                SubjectId = x.SubjectId,

                SubjectName =
                    x.Subject.Name
            });
        }

        public async Task<IEnumerable<TeachingAssignmentDto>> GetTeachingAssignmentsAsync()
        {
            var assignments =
                await _assignmentRepository
                    .GetTeachingAssignmentsAsync();


            return assignments.Select(x => new TeachingAssignmentDto
            {
                TeacherId = x.TeacherId,

                TeacherName =
                    $"{x.Teacher.FirstName} {x.Teacher.LastName}",

                SubjectId = x.SubjectId,

                SubjectName =
                    x.Subject.Name,

                SchoolClassId =
                    x.SchoolClassId,

                SchoolClassName =
                    x.SchoolClass.Name
            });
        }
    }
}
