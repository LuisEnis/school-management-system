namespace SchoolManagement.API.DTOs.Teacher
{
    public class TeacherAssignmentDto
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;
    }
}
