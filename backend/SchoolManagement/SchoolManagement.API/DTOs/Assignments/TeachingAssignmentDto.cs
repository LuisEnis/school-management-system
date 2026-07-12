using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Assignments
{
    public class TeachingAssignmentDto
    {
        [Range(1, int.MaxValue)]
        public int ClassId { get; set; }

        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }

        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
    }
}
