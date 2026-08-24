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
            var totalStudents =
                await _userRepository.CountByRoleAsync(UserRole.Student);

            var totalTeachers =
                await _userRepository.CountByRoleAsync(UserRole.Teacher);

            var totalClasses =
                await _schoolClassRepository.CountAsync();

            var totalSubjects =
                await _subjectRepository.CountAsync();

            var studentClassAssignments =
                await _assignmentRepository.GetStudentClassAssignmentsAsync();

            var teacherSubjectAssignments =
                await _assignmentRepository.GetTeacherSubjectAssignmentsAsync();

            var teachingAssignments =
                await _assignmentRepository.GetTeachingAssignmentsAsync();

            return new ManagementDashboardDto
            {
                TotalStudents = totalStudents,

                TotalTeachers = totalTeachers,

                TotalClasses = totalClasses,

                TotalSubjects = totalSubjects,

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
