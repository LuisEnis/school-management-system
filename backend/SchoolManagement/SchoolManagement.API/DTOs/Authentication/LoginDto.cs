using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.API.DTOs.Authentication
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
