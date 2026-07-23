using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public TeacherService(ITeacherRepository teacherRepository, IAssignmentRepository assignmentRepository)
        {
            _teacherRepository = teacherRepository;
            _assignmentRepository = assignmentRepository;
        }


        public async Task<IEnumerable<TeacherAssignmentDto>> GetClassesAsync(int teacherId)
        {
            return await _teacherRepository
                .GetTeacherClassesAsync(teacherId);
        }

        public async Task<IEnumerable<UserDto>> GetStudentsByClassAsync(int teacherId, int classId)
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


            return await _teacherRepository
                .GetStudentsByClassAsync(classId);
        }
    }
}
