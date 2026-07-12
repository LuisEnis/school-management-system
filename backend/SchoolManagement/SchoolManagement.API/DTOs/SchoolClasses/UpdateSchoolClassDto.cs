using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.SchoolClasses
{
    public class UpdateSchoolClassDto
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
