using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Assignments
{
    public class CreateTeacherSubjectAssignmentDto
    {
        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }

        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }
    }
}
