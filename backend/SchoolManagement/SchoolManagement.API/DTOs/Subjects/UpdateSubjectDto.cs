using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Subjects
{
    public class UpdateSubjectDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
