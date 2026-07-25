using AutoMapper;
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
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IAssignmentRepository assignmentRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TeacherAssignmentDto>> GetClassesAsync(int teacherId)
        {
            var assignments = await _teacherRepository
                .GetTeacherClassesAsync(teacherId);

            return _mapper.Map<IEnumerable<TeacherAssignmentDto>>(assignments);
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

            var students = await _teacherRepository
                .GetStudentsByClassAsync(classId);

            return _mapper.Map<IEnumerable<UserDto>>(students);
        }
    }
}
