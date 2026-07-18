using SchoolManagement.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Users
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
