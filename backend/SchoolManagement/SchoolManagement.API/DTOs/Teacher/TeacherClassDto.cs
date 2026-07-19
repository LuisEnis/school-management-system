namespace SchoolManagement.API.DTOs.Teacher
{
    public class TeacherClassDto
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;
    }
}
