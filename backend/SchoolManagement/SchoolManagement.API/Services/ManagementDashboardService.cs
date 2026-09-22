using SchoolManagement.API.Caching;
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
        private readonly ICacheService _cacheService;

        public ManagementDashboardService(
            IUserRepository userRepository,
            ISchoolClassRepository schoolClassRepository,
            ISubjectRepository subjectRepository,
            IAssignmentRepository assignmentRepository,
            ICacheService cacheService)
        {
            _userRepository = userRepository;
            _schoolClassRepository = schoolClassRepository;
            _subjectRepository = subjectRepository;
            _assignmentRepository = assignmentRepository;
            _cacheService = cacheService;
        }

        public async Task<ManagementDashboardDto> GetDashboardAsync()
        {

            if (_cacheService.TryGetValue(CacheKeys.ManagementDashboard, out ManagementDashboardDto? cachedDashboard) && cachedDashboard != null)
            {
                return cachedDashboard;
            }

            var totalStudents =
                await _userRepository.CountByRoleAsync(UserRole.Student);

            var totalTeachers =
                await _userRepository.CountByRoleAsync(UserRole.Teacher);

            var totalClasses =
                await _schoolClassRepository.CountAsync();

            var totalSubjects =
                await _subjectRepository.CountAsync();

            var totalStudentClassAssignments =
                await _assignmentRepository.CountStudentClassAssignmentsAsync();

            var totalTeacherSubjectAssignments =
                await _assignmentRepository.CountTeacherSubjectAssignmentsAsync();

            var totalTeachingAssignments =
                await _assignmentRepository.CountTeachingAssignmentsAsync();

            var dashboard = new ManagementDashboardDto
            {
                TotalStudents = totalStudents,

                TotalTeachers = totalTeachers,

                TotalClasses = totalClasses,

                TotalSubjects = totalSubjects,

                TotalStudentClassAssignments = totalStudentClassAssignments,

                TotalTeacherSubjectAssignments = totalTeacherSubjectAssignments,

                TotalTeachingAssignments = totalTeachingAssignments
            };

            _cacheService.Set(CacheKeys.ManagementDashboard, dashboard, CacheDurations.ManagementDashboard);

            return dashboard;
        }
    }
}
