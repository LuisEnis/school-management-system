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


        public TeacherService(
            ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }


        public async Task<IEnumerable<TeacherClassDto>> GetClassesAsync(int teacherId)
        {
            return await _teacherRepository
                .GetTeacherClassesAsync(teacherId);
        }

        public async Task<IEnumerable<UserDto>> GetStudentsByClassAsync(int teacherId, int classId)
        {
            var teacherHasClass =
                await _teacherRepository
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
