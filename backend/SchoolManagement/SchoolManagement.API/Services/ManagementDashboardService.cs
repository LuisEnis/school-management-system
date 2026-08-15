using SchoolManagement.API.DTOs.ManagementDashboard;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class ManagementDashboardService : IManagementDashboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public ManagementDashboardService(
            IUserRepository userRepository,
            ISchoolClassRepository schoolClassRepository,
            ISubjectRepository subjectRepository,
            IAssignmentRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _schoolClassRepository = schoolClassRepository;
            _subjectRepository = subjectRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<ManagementDashboardDto> GetDashboardAsync()
        {
            var students =
                await _userRepository.GetByRoleAsync(UserRole.Student);

            var teachers =
                await _userRepository.GetByRoleAsync(UserRole.Teacher);

            var classes =
                await _schoolClassRepository.GetAllAsync();

            var subjects =
                await _subjectRepository.GetAllAsync();

            var studentClassAssignments =
                await _assignmentRepository.GetStudentClassAssignmentsAsync();

            var teacherSubjectAssignments =
                await _assignmentRepository.GetTeacherSubjectAssignmentsAsync();

            var teachingAssignments =
                await _assignmentRepository.GetTeachingAssignmentsAsync();

            return new ManagementDashboardDto
            {
                TotalStudents = students.Count(),

                TotalTeachers = teachers.Count(),

                TotalClasses = classes.Count(),

                TotalSubjects = subjects.Count(),

                TotalStudentClassAssignments =
                    studentClassAssignments.Count(),

                TotalTeacherSubjectAssignments =
                    teacherSubjectAssignments.Count(),

                TotalTeachingAssignments =
                    teachingAssignments.Count()
            };
        }
    }
}
