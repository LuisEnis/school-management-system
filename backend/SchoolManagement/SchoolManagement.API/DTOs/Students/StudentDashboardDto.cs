using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.DTOs.Students
{
    public class StudentDashboardDto
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public List<StudentSubjectDto> Subjects { get; set; } = new();
    }
}
