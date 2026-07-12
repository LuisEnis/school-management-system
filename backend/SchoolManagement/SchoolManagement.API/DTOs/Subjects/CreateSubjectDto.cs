using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Subjects
{
    public class CreateSubjectDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
