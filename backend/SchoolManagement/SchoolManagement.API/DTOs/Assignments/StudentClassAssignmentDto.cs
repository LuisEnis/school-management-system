using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Assignments
{
    public class StudentClassAssignmentDto
    {
        [Range(1, int.MaxValue)]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue)]
        public int ClassId { get; set; }
    }
}
