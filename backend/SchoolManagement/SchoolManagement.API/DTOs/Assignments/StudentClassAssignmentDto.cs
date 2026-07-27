namespace SchoolManagement.API.DTOs.Assignments
{
    public class StudentClassAssignmentDto
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;


        public int SchoolClassId { get; set; }

        public string SchoolClassName { get; set; } = string.Empty;
    }
}
