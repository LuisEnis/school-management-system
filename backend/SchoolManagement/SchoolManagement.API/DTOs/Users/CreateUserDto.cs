using SchoolManagement.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Users
{
    public class CreateUserDto
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
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
