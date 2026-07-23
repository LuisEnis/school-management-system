using SchoolManagement.API.DTOs.Students;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;


        public StudentService(
            IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        public async Task<StudentDashboardDto?> GetDashboardAsync(int studentId)
        {
            var studentClass =
                await _studentRepository
                    .GetStudentClassAsync(studentId);


            if (studentClass == null)
                return null;


            var schoolClass =
                studentClass.SchoolClass;


            return new StudentDashboardDto
            {
                ClassId = schoolClass.Id,

                ClassName = schoolClass.Name,

                Subjects =
                    schoolClass.TeachingAssignments
                    .Select(ta => new StudentSubjectDto
                    {
                        SubjectName = ta.Subject.Name,

                        TeacherName =
                            $"{ta.Teacher.FirstName} {ta.Teacher.LastName}"
                    })
                    .ToList()
            };
        }
    }
}
