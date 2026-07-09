using SchoolManagement.API.Enums;

namespace SchoolManagement.API.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }


        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();

        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

        public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
    }
}
