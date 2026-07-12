using SchoolManagement.API.Enums;

namespace SchoolManagement.API.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}