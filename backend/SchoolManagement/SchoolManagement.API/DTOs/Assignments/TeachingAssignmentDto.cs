namespace SchoolManagement.API.DTOs.Assignments
{
    public class TeachingAssignmentDto
    {
        public int TeacherId { get; set; }

        public string TeacherName { get; set; } = string.Empty;


        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;


        public int SchoolClassId { get; set; }

        public string SchoolClassName { get; set; } = string.Empty;
    }
}
