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


        public AssignmentService(
            IAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }


        public async Task AssignStudentToClassAsync(
            StudentClassAssignmentDto dto)
        {
            var entity = _mapper.Map<StudentClass>(dto);

            await _assignmentRepository.AddStudentClassAsync(entity);
            await _assignmentRepository.SaveChangesAsync();
        }


        public async Task AssignTeacherToSubjectAsync(
            TeacherSubjectAssignmentDto dto)
        {
            var entity = _mapper.Map<TeacherSubject>(dto);

            await _assignmentRepository.AddTeacherSubjectAsync(entity);
            await _assignmentRepository.SaveChangesAsync();
        }


        public async Task AssignTeacherToClassSubjectAsync(
            TeachingAssignmentDto dto)
        {
            var entity = _mapper.Map<TeachingAssignment>(dto);

            await _assignmentRepository.AddTeachingAssignmentAsync(entity);
            await _assignmentRepository.SaveChangesAsync();
        }


        public async Task<bool> RemoveStudentFromClassAsync(
            int studentId,
            int classId)
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


        public async Task<bool> RemoveTeacherFromSubjectAsync(
            int teacherId,
            int subjectId)
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


        public async Task<bool> RemoveTeachingAssignmentAsync(
            int classId,
            int subjectId,
            int teacherId)
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
