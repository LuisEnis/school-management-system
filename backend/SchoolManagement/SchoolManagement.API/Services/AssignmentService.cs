using AutoMapper;
using SchoolManagement.API.DTOs.Assignments;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;


        public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }


        public async Task<StudentClassAssignmentDto> AssignStudentToClassAsync(CreateStudentClassAssignmentDto dto)
        {
            var alreadyAssigned =
                await _assignmentRepository
                    .StudentAlreadyAssignedToClassAsync(
                        dto.StudentId);

            if (alreadyAssigned)
            {
                throw new Exception(
                    "Student is already assigned to a class.");
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
            var exists =
                await _assignmentRepository
                    .GetTeacherSubjectAsync(
                        dto.TeacherId,
                        dto.SubjectId);


            if (exists != null)
            {
                throw new Exception(
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
            var teacherCanTeach =
                await _assignmentRepository
                    .TeacherCanTeachSubjectAsync(
                        dto.TeacherId,
                        dto.SubjectId);


            if (!teacherCanTeach)
            {
                throw new Exception(
                    "Teacher is not assigned to this subject.");
            }


            var subjectAlreadyAssigned =
                await _assignmentRepository
                    .TeachingAssignmentExistsForClassAsync(
                        dto.SchoolClassId,
                        dto.SubjectId);


            if (subjectAlreadyAssigned)
            {
                throw new Exception(
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
                    .GetTeacherSubjectAsync(teacherId, subjectId);

            if (assignment == null)
                return false;

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
    }
}
