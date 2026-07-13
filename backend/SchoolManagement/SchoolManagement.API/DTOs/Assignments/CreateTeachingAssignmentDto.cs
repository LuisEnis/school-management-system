using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Assignments
{
    public class CreateTeachingAssignmentDto
    {
        [Range(1, int.MaxValue)]
        public int SchoolClassId { get; set; }

        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }

        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
    }
}
