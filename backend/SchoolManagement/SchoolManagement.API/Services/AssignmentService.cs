using AutoMapper;
using SchoolManagement.API.DTOs.Assignments;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Exceptions;
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


        public AssignmentService(IAssignmentRepository assignmentRepository, IUserRepository userRepository, ISubjectRepository subjectRepository, ISchoolClassRepository schoolClassRepository, IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
            _schoolClassRepository = schoolClassRepository;
            _mapper = mapper;
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

            return _mapper.Map<StudentClassAssignmentDto>(entity);
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

            return _mapper.Map<TeacherSubjectAssignmentDto>(entity);
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

            return _mapper.Map<TeachingAssignmentDto>(entity);
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
